
namespace CirclesFundMe.Application.CQRS.QueryHandlers.Users
{
    public class GetMyProfileQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, UtilityHelper utility) : IRequestHandler<GetMyProfileQuery, BaseResponse<UserModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly UtilityHelper _utility = utility;

        public async Task<BaseResponse<UserModel>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            string userId = _currentUserService.UserId;

            AppUserExtension? user = await _unitOfWork.Users.GetUserByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                return BaseResponse<UserModel>.NotFound("User not found.");
            }

            UserModel userModel = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                ContributionScheme = user.UserContributionScheme != null ? new ContributionSchemeMiniModel
                {
                    Id = user.UserContributionScheme.Id,
                    Name = user.UserContributionScheme.ContributionScheme?.Name,
                    Type = user.UserContributionScheme.ContributionScheme?.SchemeType.ToString(),
                } : null,
                WithdrawalSetting = user.WithdrawalSetting != null ? new WithdrawalSettingModel
                {
                    Id = user.WithdrawalSetting.Id,
                    AccountNumber = user.WithdrawalSetting.AccountNumber,
                    AccountName = user.WithdrawalSetting.AccountName,
                    BankCode = user.WithdrawalSetting.BankCode
                } : null,
                ContributionAmount = user.UserContributionScheme?.ContributionAmount,
                IncomeAmount = user.UserContributionScheme?.IncomeAmount,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                OnboardingStatus = user.OnboardingStatus.ToString(),
                AllowPushNotifications = user.AllowPushNotifications,
                AllowEmailNotifications = user.AllowEmailNotifications,
                IsPaymentSetupComplete = user.IsPaymentSetupComplete,
                Gender = user.Gender.ToString(),
                AutoLoanDetail = user.UserContributionScheme!.ContributionScheme!.SchemeType == SchemeTypeEnums.AutoFinance
                    ? GetMyAutoLoanDetail(user.UserContributionScheme.CopyOfCurrentBreakdownAtOnboarding, user.UserContributionScheme.ContributionAmount) : null
            };

            return BaseResponse<UserModel>.Success(userModel, "User retrieved successfully.");
        }

        private MyAutoLoanDetail GetMyAutoLoanDetail(string? copyOfCurrentBreakdownAtOnboarding, decimal contributionAmount)
        {
            AutoFinanceBreakdown? breakdown = copyOfCurrentBreakdownAtOnboarding != null
                ? _utility.Deserializer<AutoFinanceBreakdown>(copyOfCurrentBreakdownAtOnboarding)
                : null;

            if (breakdown == null)
            {
                return new MyAutoLoanDetail
                {
                    CostOfVehicle = 0,
                    PreLoanContributionAmount = 0,
                    PostLoanWeeklyContribution = 0
                };
            }
            return new MyAutoLoanDetail
            {
                CostOfVehicle = breakdown.CostOfVehicle,
                PreLoanContributionAmount = Math.Round(contributionAmount, 2),
                PostLoanWeeklyContribution = Math.Round(breakdown.PostLoanWeeklyContribution, 2)
            };
        }
    }
}

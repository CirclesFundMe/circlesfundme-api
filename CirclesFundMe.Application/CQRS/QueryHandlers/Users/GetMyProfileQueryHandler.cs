namespace CirclesFundMe.Application.CQRS.QueryHandlers.Users
{
    public class GetMyProfileQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<GetMyProfileQuery, BaseResponse<UserModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<UserModel>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            string userId = _currentUserService.UserId;

            AppUser? user = await _unitOfWork.Users.GetUserByIdAsync(userId, cancellationToken);

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
                ContributionAmount = user.UserContributionScheme?.ContributionAmount,
                IncomeAmount = user.UserContributionScheme?.IncomeAmount,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                OnboardingStatus = user.OnboardingStatus.ToString(),
                AllowPushNotifications = user.AllowPushNotifications,
                AllowEmailNotifications = user.AllowEmailNotifications,
                IsPaymentSetupComplete = user.IsPaymentSetupComplete,
                Gender = user.Gender.ToString()
            };

            return BaseResponse<UserModel>.Success(userModel, "User retrieved successfully.");
        }
    }
}

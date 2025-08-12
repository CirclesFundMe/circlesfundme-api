namespace CirclesFundMe.Application.CQRS.QueryHandlers.Users
{
    public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<GetUserByIdQuery, BaseResponse<UserModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<UserModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != _currentUserService.UserId && !_currentUserService.IsAdmin)
            {
                return BaseResponse<UserModel>.Forbidden("You do not have permission to access this user.");
            }

            AppUserExtension? user = await _unitOfWork.Users.GetUserByIdAsync(request.UserId, cancellationToken);

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
                UserDocuments = user.UserDocuments.Select(doc => new UserDocumentModel
                {
                    DocumentType = doc.DocumentType.ToString(),
                    DocumentUrl = doc.DocumentUrl,
                    DocumentName = doc.DocumentName,
                }).ToList(),
                ContributionAmount = user.UserContributionScheme?.ContributionAmount,
                IncomeAmount = user.UserContributionScheme?.IncomeAmount,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                OnboardingStatus = user.OnboardingStatus.ToString(),
                AllowPushNotifications = user.AllowPushNotifications,
                AllowEmailNotifications = user.AllowEmailNotifications,
                Gender = user.Gender.ToString(),
                BVN = user.UserKYC?.BVN,
                IsActive = !user.IsDeleted
            };

            return BaseResponse<UserModel>.Success(userModel, "User retrieved successfully.");
        }
    }
}

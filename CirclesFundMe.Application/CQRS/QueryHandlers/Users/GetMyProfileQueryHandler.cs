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
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                MiddleName = user.MiddleName,
                UserType = user.UserType,
                ProfilePictureUrl = user.ProfilePictureUrl,
                AllowPushNotifications = user.AllowPushNotifications,
                OnboardingStatus = user.OnboardingStatus.ToString(),
            };

            return BaseResponse<UserModel>.Success(userModel, "User retrieved successfully.");
        }
    }
}

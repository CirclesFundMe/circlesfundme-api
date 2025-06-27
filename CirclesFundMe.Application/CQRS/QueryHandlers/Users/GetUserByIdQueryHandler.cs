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

            AppUser? user = await _unitOfWork.Users.GetUserByIdAsync(request.UserId, cancellationToken);

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
                MiddleName = user.MiddleName,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return BaseResponse<UserModel>.Success(userModel, "User retrieved successfully.");
        }
    }
}

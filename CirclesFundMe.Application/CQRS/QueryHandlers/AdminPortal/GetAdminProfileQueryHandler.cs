namespace CirclesFundMe.Application.CQRS.QueryHandlers.AdminPortal
{
    public class GetAdminProfileQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<GetAdminProfileQuery, BaseResponse<AdminProfileModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<AdminProfileModel>> Handle(GetAdminProfileQuery request, CancellationToken cancellationToken)
        {
            string userId = _currentUserService.UserId;

            AppUserExtension? user = await _unitOfWork.Users.GetUserByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                return BaseResponse<AdminProfileModel>.NotFound("User not found.");
            }

            AdminProfileModel userModel = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return BaseResponse<AdminProfileModel>.Success(userModel, "User retrieved successfully.");
        }
    }
}

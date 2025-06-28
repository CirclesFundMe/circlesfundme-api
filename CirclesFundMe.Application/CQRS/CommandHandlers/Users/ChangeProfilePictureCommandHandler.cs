namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class ChangeProfilePictureCommandHandler(UserManager<AppUser> userManager, ICurrentUserService currentUserService, IImageService imageService) : IRequestHandler<ChangeProfilePictureCommand, BaseResponse<bool>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IImageService _imageService = imageService;

        public async Task<BaseResponse<bool>> Handle(ChangeProfilePictureCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                return BaseResponse<bool>.NotFound("User not found.");
            }

            string? profilePictureUrl = await _imageService.UploadImage(request.ProfilePicture);
            if (string.IsNullOrEmpty(profilePictureUrl))
            {
                return BaseResponse<bool>.BadRequest("Failed to upload profile picture.");
            }

            user.ProfilePictureUrl = profilePictureUrl;
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return BaseResponse<bool>.Success(true, "Profile picture updated successfully.");
            }
            else
            {
                return BaseResponse<bool>.BadRequest("Failed to update profile picture: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}

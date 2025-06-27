namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class ChangePasswordCommandHandler(UserManager<AppUser> userManager, IOTPService oTPService, ICurrentUserService currentUserService) : IRequestHandler<ChangePasswordCommand, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IOTPService _oTPService = oTPService;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            string email = _currentUserService.UserEmail.Trim().ToLower();

            AppUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BaseResponse<string>.NotFound($"User with email {email} not found");
            }

            if (!await _oTPService.VerifyOtp(email, request.OTP, cancellationToken))
            {
                return BaseResponse<string>.BadRequest("Invalid OTP");
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return new BaseResponse<string>
                {
                    Message = "Your password has been changed successfully"
                };
            }
            else
            {
                string errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return BaseResponse<string>.BadRequest($"Password change failed: {errorMessage}");
            }
        }
    }
}

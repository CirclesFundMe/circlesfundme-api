namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class ResetPasswordCommandHandler(UserManager<AppUser> userManager, IOTPService oTPService) : IRequestHandler<ResetPasswordCommand, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IOTPService _oTPService = oTPService;

        public async Task<BaseResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return BaseResponse<string>.NotFound("User not found");
            }

            (bool isValid, string message) = await _oTPService.ValidateOtp(request.Email.Trim().ToLower(), request.Otp.Trim(), cancellationToken);

            if (!isValid)
            {
                return BaseResponse<string>.BadRequest(message);
            }

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult userPasswordReset = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (userPasswordReset.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return new()
                {
                    Message = "Your password has been reset successfully"
                };
            }
            else
            {
                string err = string.Join(',', userPasswordReset.Errors.Select(x => x.Description));
                return BaseResponse<string>.BadRequest($"Password reset failed: {err}");
            }
        }
    }
}

namespace CirclesFundMe.API.Controllers.v1
{
    public class AuthController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpPost("login")]
        [ProducesResponseType<BaseResponse<LoginModel>>(200)]
        [SwaggerOperation(Summary = "Login")]
        public async Task<IActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<LoginModel> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType<BaseResponse<string>>(200)]
        [SwaggerOperation(Summary = "Forgot Password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<string> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("reset-password")]
        [ProducesResponseType<BaseResponse<string>>(200)]
        [SwaggerOperation(Summary = "Reset Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<string> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("verify-otp")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Verify OTP")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("resend-otp")]
        [ProducesResponseType<BaseResponse<string>>(200)]
        [SwaggerOperation(Summary = "Resend OTP")]
        public async Task<IActionResult> ResendOtp(ResendOtpCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<string> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType<BaseResponse<LoginModel>>(200)]
        [SwaggerOperation(Summary = "Refresh Token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<LoginModel> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }
    }
}

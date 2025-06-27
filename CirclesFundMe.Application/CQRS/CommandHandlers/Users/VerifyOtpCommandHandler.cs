namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class VerifyOtpCommandHandler(IOTPService oTPService) : IRequestHandler<VerifyOtpCommand, BaseResponse<bool>>
    {
        private readonly IOTPService _oTPService = oTPService;

        public async Task<BaseResponse<bool>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            bool isValid = await _oTPService.VerifyOtp(request.Email.Trim().ToLower(), request.Otp.Trim(), cancellationToken);

            if (!isValid)
            {
                return BaseResponse<bool>.BadRequest("Invalid OTP");
            }

            return new()
            {
                Data = true,
                Message = "OTP verified successfully"
            };
        }
    }
}

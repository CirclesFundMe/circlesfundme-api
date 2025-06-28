namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class SendOTPCommandHandler(IOTPService oTPService) : IRequestHandler<SendOTPCommand, BaseResponse<bool>>
    {
        private readonly IOTPService _oTPService = oTPService;

        public async Task<BaseResponse<bool>> Handle(SendOTPCommand request, CancellationToken cancellationToken)
        {
            string otp = UtilityHelper.GenerateOtp();

            (bool result, string message) = await _oTPService.SendOtp(request.Email, otp, null, cancellationToken);

            if (!result)
            {
                return BaseResponse<bool>.BadRequest(message);
            }

            return BaseResponse<bool>.Success(true, "A one-time-password has been sent");
        }
    }
}

using CirclesFundMe.Application.HttpClients.Paystack;

namespace CirclesFundMe.API.Controllers.v1
{
    public class AccountsController(ISender sender, IPaystackClient paystackClient) : BaseControllerV1
    {
        private readonly ISender _sender = sender;
        private readonly IPaystackClient _paystackClient = paystackClient;

        [HttpPost("send-onboarding-otp")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Send One-Time Password (OTP) to Email for Onboarding")]
        public async Task<IActionResult> SendOtp(SendOTPCommand command, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }

        [HttpPost("create-new")]
        [ProducesResponseType<BaseResponse<string>>(200)]
        [SwaggerOperation(Summary = "Create New CirclesFundMe Account")]
        public async Task<IActionResult> CreateNew(CreateNewUserAccountCommand command, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }

        [HttpPost("complete-onboarding")]
        [ProducesResponseType<BaseResponse<UserModel>>(200)]
        [SwaggerOperation(Summary = "Complete CirclesFundMe Account Onboarding")]
        [Authorize]
        public async Task<IActionResult> CompleteAccountCreation([FromForm] CompleteUserOnboardingCommand command, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }

        [HttpGet("banks")]
        [SwaggerOperation(Summary = "Get List of Banks")]
        public async Task<IActionResult> GetBanks([FromQuery] BankDataQuery query, CancellationToken cancellation)
        {
            var response = await _paystackClient.GetBanksList(query, cancellation);
            return Ok(response);
        }
    }
}

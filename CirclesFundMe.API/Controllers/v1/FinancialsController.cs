namespace CirclesFundMe.API.Controllers.v1
{
    public class FinancialsController(ISender sender, IConfiguration config, ILogger<FinancialsController> logger, UtilityHelper utility) : BaseControllerV1
    {
        private readonly ISender _sender = sender;
        private readonly string _paystackWebhookSecret = config["PaystackService:SecretKey"] ?? string.Empty;
        private readonly ILogger<FinancialsController> _logger = logger;
        private readonly UtilityHelper _utility = utility;

        [HttpGet("banks")]
        [ProducesResponseType<BaseResponse<IEnumerable<BankModel>>>(200)]
        [SwaggerOperation(Summary = "Get Banks")]
        public async Task<IActionResult> GetBanks(CancellationToken cancellationToken)
        {
            BaseResponse<IEnumerable<BankModel>> response = await _sender.Send(new GetBankQuery(), cancellationToken);
            return HandleResponse(response);
        }

        [HttpGet("account-name-enquiry")]
        [ProducesResponseType<BaseResponse<string>>(200)]
        [SwaggerOperation(Summary = "Get Account Name")]
        public async Task<IActionResult> GetAccountName([FromQuery] AccountNameEnquiryQuery query, CancellationToken cancellationToken)
        {
            BaseResponse<string> response = await _sender.Send(query, cancellationToken);
            return HandleResponse(response);
        }

        [HttpGet("my-wallets")]
        [ProducesResponseType<BaseResponse<List<WalletModel>>>(200)]
        [SwaggerOperation(Summary = "Get My Wallets")]
        [Authorize]
        public async Task<IActionResult> GetMyWallets(CancellationToken cancellationToken)
        {
            BaseResponse<List<WalletModel>> response = await _sender.Send(new GetMyWalletsQuery(), cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("withdraw-contribution")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Withdraw Contribution")]
        [Authorize]
        public async Task<IActionResult> WithdrawContribution([FromBody] WithdrawContributionCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("make-initial-contribution")]
        [ProducesResponseType<BaseResponse<InitializeTransactionModel>>(200)]
        [SwaggerOperation(Summary = "Make Initial Contribution")]
        [Authorize]
        public async Task<IActionResult> MakeInitialContribution([FromBody] MakeInitialContributionCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<InitializeTransactionModel> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("paystack-webhook")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Paystack Webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> PaystackWebhook()
        {
            using StreamReader reader = new(Request.Body);
            string? requestBody = await reader.ReadToEndAsync();

            _logger.LogInformation("Received Paystack webhook: {RequestBody}", requestBody);

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return BadRequest("Request body is empty");
            }

            string? signature = Request.Headers["x-paystack-signature"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(signature))
            {
                _logger.LogWarning("Missing Paystack signature header");
                return Unauthorized("Missing Paystack signature header");
            }

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_paystackWebhookSecret));
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestBody));
            string computedSignature = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

            if (!string.Equals(signature, computedSignature, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Invalid Paystack signature: {Signature} vs Computed: {ComputedSignature}", signature, computedSignature);
                //return Unauthorized("Invalid signature");
            }

            PaystackWebhookCommand? command = _utility.Deserializer<PaystackWebhookCommand>(requestBody);
            if (command == null)
            {
                _logger.LogWarning("Invalid request body: {RequestBody}", requestBody);
                return BadRequest("Invalid request body");
            }

            BaseResponse<bool> response = await _sender.Send(command);
            return HandleResponse(response);
        }
    }
}

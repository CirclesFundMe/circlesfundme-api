namespace CirclesFundMe.API.Controllers.v1
{
    public class FinancialsController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

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
    }
}

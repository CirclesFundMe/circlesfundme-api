namespace CirclesFundMe.API.Controllers.v1
{
    public class LoanApplicationsController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpGet]
        [ProducesResponseType<BaseResponse<PagedList<LoanApplicationModel>>>(200)]
        [SwaggerOperation(Summary = "Get Loan Applications")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> GetLoanApplications([FromQuery] LoanApplicationParams loanApplicationParams, CancellationToken cancellationToken)
        {
            BaseResponse<PagedList<LoanApplicationModel>> response = await _sender.Send(new GetLoanApplicationsQuery() { LoanApplicationParams = loanApplicationParams }, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("create")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Create Loan Application")]
        public async Task<IActionResult> CreateLoanApplication(CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = await _sender.Send(new CreateLoanApplicationCommand(), cancellationToken);
            return HandleResponse(response);
        }
    }
}

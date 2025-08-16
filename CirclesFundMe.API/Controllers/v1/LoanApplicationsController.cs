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

        [HttpGet("{loanApplicationId:guid}")]
        [ProducesResponseType<BaseResponse<LoanApplicationDetailModel>>(200)]
        [SwaggerOperation(Summary = "Get Loan Application By Id")]
        public async Task<IActionResult> GetLoanApplicationById(Guid loanApplicationId, CancellationToken cancellationToken)
        {
            BaseResponse<LoanApplicationDetailModel> response = await _sender.Send(new GetLoanApplicationByIdQuery() { LoanApplicationId = loanApplicationId }, cancellationToken);
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

        [HttpPost("reject")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Reject Loan Application")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> RejectLoanApplication([FromBody] RejectLoanApplicationCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("waitlist")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Waitlist Loan Application")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> WaitlistLoanApplication([FromBody] WaitlistLoanApplicationCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("approve")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Approve Loan Application")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> ApproveLoanApplication([FromBody] ApproveLoanApplicationCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }
    }
}

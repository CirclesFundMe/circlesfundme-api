namespace CirclesFundMe.API.Controllers.v1
{
    public class ContributionSchemesController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpGet]
        [ProducesResponseType<BaseResponse<List<ContributionSchemeModel>>>(200)]
        [SwaggerOperation(Summary = "Get Contribution Schemes")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> GetContributionSchemes(CancellationToken cancellationToken)
        {
            BaseResponse<List<ContributionSchemeModel>> response = await _sender.Send(new GetContributionSchemesQuery(), cancellationToken);
            return HandleResponse(response);
        }
        
        [HttpGet("mini")]
        [ProducesResponseType<BaseResponse<List<ContributionSchemeMiniModel>>>(200)]
        [SwaggerOperation(Summary = "Get Contribution Schemes Mini Data for Onboarding")]
        public async Task<IActionResult> GetContributionSchemesMini(CancellationToken cancellationToken)
        {
            BaseResponse<List<ContributionSchemeMiniModel>> response = await _sender.Send(new GetContributionSchemesMiniQuery(), cancellationToken);
            return HandleResponse(response);
        }

        [HttpGet("{contributionSchemeId}/eligible-loan-detail")]
        [ProducesResponseType<BaseResponse<EligibleLoanDetailModel>>(200)]
        [SwaggerOperation(Summary = "Get Eligible Loan Detail by Contribution Scheme ID")]
        public async Task<IActionResult> GetEligibleLoanDetail(Guid contributionSchemeId, CancellationToken cancellationToken)
        {
            BaseResponse<EligibleLoanDetailModel> response = await _sender.Send(new GetEligibleLoanDetailQuery() { ContributionSchemeId = contributionSchemeId }, cancellationToken);
            return HandleResponse(response);
        }
    }
}

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

        [HttpPut("{contributionSchemeId}")]
        [ProducesResponseType<BaseResponse<ContributionSchemeModel>>(200)]
        [SwaggerOperation(Summary = "Update Contribution Scheme")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> UpdateContributionScheme(Guid contributionSchemeId, UpdateContributionSchemeCommand command, CancellationToken cancellationToken)
        {
            command.ContributionSchemeId = contributionSchemeId;
            BaseResponse<ContributionSchemeModel> response = await _sender.Send(command, cancellationToken);
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

        [HttpPost("regular-finance-breakdown")]
        [ProducesResponseType<BaseResponse<RegularLoanBreakdownModel>>(200)]
        [SwaggerOperation(Summary = "Get Eligible Loan Detail by Contribution Scheme ID")]
        public async Task<IActionResult> GetEligibleLoanDetail([FromBody] GetEligibleLoanDetailQuery query, CancellationToken cancellationToken)
        {
            BaseResponse<RegularLoanBreakdownModel> response = await _sender.Send(query, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("auto-finance-breakdown")]
        [ProducesResponseType<BaseResponse<AutoFinanceBreakdownModel>>(200)]
        [SwaggerOperation(Summary = "Get Auto Finance Breakdown")]
        public async Task<IActionResult> GetAutoFinanceBreakdown([FromBody] GetAutoFinanceBreakdownQuery query, CancellationToken cancellationToken)
        {
            BaseResponse<AutoFinanceBreakdownModel> response = await _sender.Send(query, cancellationToken);
            return HandleResponse(response);
        }
    }
}

namespace CirclesFundMe.Application.CQRS.Queries.Contributions
{
    public record GetEligibleLoanDetailQuery : IRequest<BaseResponse<RegularLoanBreakdownModel>>
    {
        public Guid ContributionSchemeId { get; set; }
        public decimal Amount { get; set; }
    }
}

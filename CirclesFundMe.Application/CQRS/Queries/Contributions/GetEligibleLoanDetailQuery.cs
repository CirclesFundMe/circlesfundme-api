namespace CirclesFundMe.Application.CQRS.Queries.Contributions
{
    public record GetEligibleLoanDetailQuery : IRequest<BaseResponse<EligibleLoanDetailModel>>
    {
        public Guid ContributionSchemeId { get; set; }
    }
}

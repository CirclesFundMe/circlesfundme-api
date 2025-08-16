namespace CirclesFundMe.Application.CQRS.Queries.Loans
{
    public record GetMyLoanHistoryQuery : IRequest<BaseResponse<PagedList<LoanHistoryModel>>>
    {
        public required ApprovedLoanParams Params { get; init; }
    }
}

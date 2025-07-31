namespace CirclesFundMe.Application.CQRS.Queries.Loans
{
    public record GetLoanApplicationsQuery : IRequest<BaseResponse<PagedList<LoanApplicationModel>>>
    {
        public required LoanApplicationParams LoanApplicationParams { get; init; }
    }
}

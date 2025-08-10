namespace CirclesFundMe.Application.CQRS.Queries.Loans
{
    public record GetLoanApplicationByIdQuery : IRequest<BaseResponse<LoanApplicationDetailModel>>
    {
        public Guid LoanApplicationId { get; init; }
    }
}

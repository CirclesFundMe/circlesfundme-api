namespace CirclesFundMe.Application.CQRS.Commands.Loans
{
    public record ApproveLoanApplicationCommand : IRequest<BaseResponse<bool>>
    {
        public Guid LoanApplicationId { get; set; }
    }
}

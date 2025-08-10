namespace CirclesFundMe.Application.CQRS.Commands.Loans
{
    public record WaitlistLoanApplicationCommand : IRequest<BaseResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}

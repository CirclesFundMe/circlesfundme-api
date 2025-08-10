namespace CirclesFundMe.Application.CQRS.Commands.Loans
{
    public record RejectLoanApplicationCommand : IRequest<BaseResponse<bool>>
    {
        public Guid Id { get; set; }
        public string? RejectionReason { get; set; }
    }
}

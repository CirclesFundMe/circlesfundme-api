namespace CirclesFundMe.Application.CQRS.Commands.Finances
{
    public record WithdrawContributionCommand : IRequest<BaseResponse<bool>>
    {
        public decimal Amount { get; init; }
        public bool DeductChargeFromBalance { get; init; }
    }
}

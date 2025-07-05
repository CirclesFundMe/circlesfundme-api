namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record CreateWithdrawalSettingCommand : IRequest<BaseResponse<bool>>
    {
        public string? AccountNumber { get; init; }
        public string? BankCode { get; init; }
    }
}

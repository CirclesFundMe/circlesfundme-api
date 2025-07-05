namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record UpdateWithdrawalSettingCommand : IRequest<BaseResponse<bool>>
    {
        public Guid WithdrawalSettingId { get; init; }
        public string? AccountNumber { get; init; }
        public string? BankCode { get; init; }
        public string? Otp { get; init; }
    }
}

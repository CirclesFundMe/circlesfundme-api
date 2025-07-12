namespace CirclesFundMe.Application.Models.Users
{
    public record WithdrawalSettingModel
    {
        public Guid Id { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankCode { get; set; }
    }
}

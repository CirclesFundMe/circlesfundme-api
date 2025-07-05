namespace CirclesFundMe.Domain.Entities.Users
{
    public record UserWithdrawalSetting : BaseEntity
    {
        public string? PaystackRecipientCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }

        public string? BankCode { get; set; }
        public virtual Bank? Bank { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}

namespace CirclesFundMe.Domain.Entities.Finances
{
    public record Wallet : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Balance { get; set; }
        public WalletTypeEnums Type { get; set; }
        public WalletStatusEnums Status { get; set; }
        public DateTime? LastTranDate { get; set; }
        public DateTime? NextTranDate { get; set; }
        public string? BlockedReason { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = [];
    }
}

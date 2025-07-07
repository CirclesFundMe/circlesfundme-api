namespace CirclesFundMe.Domain.Entities.Finances
{
    public record Transaction : BaseEntity
    {
        public string? TransactionReference { get; set; }
        public string? Narration { get; set; }
        public TransactionTypeEnums TransactionType { get; set; }
        public decimal BalanceBeforeTransaction { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public DateTime TransactionDate { get; set; }
        public TimeSpan TransactionTime { get; set; }

        public Guid WalletId { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}

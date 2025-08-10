namespace CirclesFundMe.Domain.Entities.Loans
{
    public record LoanApplication : BaseEntity
    {
        public LoanApplicationStatusEnums Status { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal CurrentEligibleAmount { get; set; }
        public SchemeTypeEnums Scheme { get; set; }
        public string? Breakdown { get; set; }
        public string? RejectionReason { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public virtual ApprovedLoan? ApprovedLoan { get; set; }
    }

    public record LoanApplicationExtension : LoanApplication
    {
        public decimal AmountRepaid { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal ApprovedAmount { get; set; }
    }
}

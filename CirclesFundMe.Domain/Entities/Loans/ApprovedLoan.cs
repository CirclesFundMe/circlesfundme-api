namespace CirclesFundMe.Domain.Entities.Loans
{
    public record ApprovedLoan : BaseEntity
    {
        public ApprovedLoanStatusEnums Status { get; set; }
        public decimal ApprovedAmount { get; set; }
        public DateTime ApprovedDate { get; set; }

        public Guid LoanApplicationId { get; set; }
        public virtual LoanApplication? LoanApplication { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public virtual ICollection<LoanRepayment> LoanRepayments { get; set; } = [];
    }

    [NotMapped]
    public record ApprovedLoanExtension : ApprovedLoan
    {
        public decimal AmountRepaid { get; set; }
        public DateTime FirstRepaymentDate { get; set; }
        public DateTime LastRepaymentDate { get; set; }
        public int RepaymentCount { get; set; }
        public int TotalRepaymentCount { get; set; }
    }
}

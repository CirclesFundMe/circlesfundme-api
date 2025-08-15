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
}

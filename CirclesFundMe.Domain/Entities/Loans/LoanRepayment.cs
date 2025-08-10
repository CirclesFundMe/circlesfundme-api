namespace CirclesFundMe.Domain.Entities.Loans
{
    public record LoanRepayment : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime? RepaymentDate { get; set; }
        public LoanRepaymentStatusEnums Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public Guid ApprovedLoanId { get; set; }
        public virtual ApprovedLoan? ApprovedLoan { get; set; }
    }
}

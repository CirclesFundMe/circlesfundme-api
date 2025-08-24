namespace CirclesFundMe.Application.Models.Loans
{
    public record LoanApplicationDetailModel
    {
        public Guid Id { get; set; }
        public string? Fullname { get; set; }
        public string? AccountNumber { get; set; }
        public string? Bank { get; set; }
        public string? Scheme { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal ContributionAmount { get; set; }
        public decimal TotalContribution { get; set; }
        public string? BVN { get; set; }

        public decimal RequestedAmount { get; set; }
        public decimal LoanManagementFee { get; set; }
        public decimal RepaymentTerm { get; set; }
        public bool IsEligible { get; set; }
        public string? Status { get; set; }
    }
}

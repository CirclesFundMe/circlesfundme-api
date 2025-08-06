namespace CirclesFundMe.Application.Models.Contributions
{
    public record RegularLoanBreakdownModel
    {
        public string? PrincipalLoan { get; set; }
        public string? PrincipalLoanDescription { get; set; }
        public string? LoanManagementFee { get; set; }
        public string? LoanManagementFeeDescription { get; set; }
        public string? EligibleLoan { get; set; }
        public string? EligibleLoanDescription { get; set; }
        public string? ServiceCharge { get; set; }
        public string? TotalRepayment { get; set; }
        public string? RepaymentTerm { get; set; }
    }
}

namespace CirclesFundMe.Application.Models.Loans
{
    public record LoanApplicationModel
    {
        public Guid Id { get; init; }
        public string? Status { get; set; }
        public string? Scheme { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal EligibleLoanAmount { get; set; }
        public decimal AmountRepaid { get; set; }
        public DateTime DateApplied { get; set; }
        public LoanApplicantDetail? ApplicantDetail { get; set; }
    }

    public record LoanApplicantDetail
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}

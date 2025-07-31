namespace CirclesFundMe.Application.Models.Loans
{
    public record LoanApplicationModel
    {
        public Guid Id { get; init; }
        public string? Status { get; set; }
        public decimal ApprovedAmount { get; set; }
        public LoanApplicantDetail? ApplicantDetail { get; set; }
    }

    public record LoanApplicantDetail
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}

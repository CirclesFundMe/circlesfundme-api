namespace CirclesFundMe.Application.Models.Contributions
{
    public record EligibleLoanDetailModel
    {
        public string? ContributionSchemeName { get; set; }
        public double EligibleLoanMultiple { get; set; }
        public string? EligibleLoanDescription { get; set; }
        public decimal ServiceChargeAmount { get; set; }
        public string? ServiceChargeDescription { get; set; }
    }
}

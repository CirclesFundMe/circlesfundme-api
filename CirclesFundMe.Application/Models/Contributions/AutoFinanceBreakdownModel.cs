namespace CirclesFundMe.Application.Models.Contributions
{
    public record AutoFinanceBreakdownModel
    {
        public decimal BaseContributionAmount { get; set; }
        public string? CostOfVehicle { get; set; }
        public string? ExtraEngine { get; set; }
        public string? ExtraTyre { get; set; }
        public string? Insurance { get; set; }
        public string? ProcessingFee { get; set; }
        public string? TotalAssetValue { get; set; }
        public string? DownPayment { get; set; }
        public string? LoanManagementFee { get; set; }
        public string? PreLoanServiceCharge { get; set; }
        public string? PostLoanWeeklyContribution { get; set; }
        public string? EligibleLoan { get; set; }
    }
}

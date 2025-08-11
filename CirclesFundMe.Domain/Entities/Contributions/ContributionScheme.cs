namespace CirclesFundMe.Domain.Entities.Contributions
{
    public record ContributionScheme : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public SchemeTypeEnums SchemeType { get; set; }
        public double MinimumVehicleCost { get; set; }

        public double ContributionPercent { get; set; }
        public double EligibleLoanMultiple { get; set; }
        public double ServiceCharge { get; set; }
        public double LoanManagementFeePercent { get; set; }
        public double DefaultPenaltyPercent { get; set; }
        public double EquityPercent { get; set; }
        public double LoanTerm { get; set; }
        public double PreLoanServiceChargePercent { get; set; }
        public double PostLoanServiceChargePercent { get; set; }

        public double ExtraEnginePercent { get; set; }
        public double ExtraTyrePercent { get; set; }
        public double InsurancePerAnnumPercent { get; set; }
        public double ProcessingFeePercent { get; set; }
        public double EligibleLoanPercent { get; set; }
        public double DownPaymentPercent { get; set; }
        public double BaseFee { get; set; }

        public virtual ICollection<UserContributionScheme> UserContributionSchemes { get; set; } = [];
    }

    [NotMapped]
    public record AutoFinanceBreakdown
    {
        public decimal CostOfVehicle { get; set; }
        public decimal ExtraEngine { get; set; }
        public decimal ExtraTyre { get; set; }
        public decimal Insurance { get; set; }
        public decimal ProcessingFee { get; set; }
        public decimal TotalAssetValue { get; set; }
        public decimal DownPayment { get; set; }
        public decimal LoanManagementFee { get; set; }
        public decimal PreLoanServiceCharge { get; set; }
        public decimal PostLoanWeeklyContribution { get; set; }
        public decimal BaseFee { get; set; }
        public decimal EligibleLoan => TotalAssetValue - DownPayment;
        public decimal TotalRepayment { get; set; }
    }

    [NotMapped]
    public record RegularFinanceBreakdown
    {
        public decimal PrincipalLoan { get; set; }
        public decimal LoanManagementFee { get; set; }
        public decimal ServiceCharge { get; set; }
        public SchemeTypeEnums SchemeType { get; set; }
        public int LoanMultiple { get; set; }
        public decimal EligibleLoan => PrincipalLoan - LoanManagementFee;
        public decimal TotalRepayment { get; set; }
        public decimal RepaymentTerm { get; set; }
        public decimal DownPayment { get; set; }
        public int CountToQualifyForLoan { get; set; }
    }
}

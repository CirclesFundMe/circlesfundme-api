namespace CirclesFundMe.Application.Models.Contributions
{
    public record ContributionSchemeModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public SchemeTypeEnums SchemeType { get; set; }

        public double ContributionPercent { get; set; }
        public double EligibleLoanMultiple { get; set; }
        public double ServiceCharge { get; set; }
        public double LoanManagementFee { get; set; }
        public double DefaultPenaltyPercent { get; set; }
        public double EquityPercent { get; set; }
        public double LoanTerm { get; set; }
        public double PreLoanServiceChargePercent { get; set; }
        public double PostLoanServiceChargePercent { get; set; }
    }
}

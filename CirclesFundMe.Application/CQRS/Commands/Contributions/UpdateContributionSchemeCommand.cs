namespace CirclesFundMe.Application.CQRS.Commands.Contributions
{
    public record UpdateContributionSchemeCommand : IRequest<BaseResponse<ContributionSchemeModel>>
    {
        public Guid ContributionSchemeId { get; set; }

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
    }
}

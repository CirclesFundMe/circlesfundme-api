namespace CirclesFundMe.Application.CQRS.Commands.Contributions
{
    public record UpdateContributionSchemeCommand : IRequest<BaseResponse<ContributionSchemeModel>>
    {
        public Guid ContributionSchemeId { get; set; }

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

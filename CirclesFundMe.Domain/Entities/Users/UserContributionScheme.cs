namespace CirclesFundMe.Domain.Entities.Users
{
    public record UserContributionScheme : BaseEntity
    {
        public decimal ContributionAmount { get; set; }
        public decimal ActualContributionAmount { get; set; }
        public decimal PreLoanChargeAmount { get; set; }
        public decimal PostLoanChargeAmount { get; set; }
        public decimal IncomeAmount { get; set; }
        public string? CopyOfCurrentBreakdownAtOnboarding { get; set; }
        public decimal MinimumContributionToQualifyForLoan { get; set; }
        public int CountToQualifyForLoan { get; set; }
        public WeekDayEnums ContributionWeekDay { get; set; }
        public MonthDayEnums ContributionMonthDay { get; set; }
        public DateTime CommencementDate { get; set; }
        public bool IsWeeklyRoutine { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public Guid ContributionSchemeId { get; set; }
        public virtual ContributionScheme? ContributionScheme { get; set; }
    }
}

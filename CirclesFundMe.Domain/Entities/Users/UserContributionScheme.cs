namespace CirclesFundMe.Domain.Entities.Users
{
    public record UserContributionScheme : BaseEntity
    {
        public decimal ContributionAmount { get; set; }
        public decimal IncomeAmount { get; set; }
        public string? CopyOfCurrentAutoBreakdownAtOnboarding { get; set; }
        public WeekDayEnums ContributionWeekDay { get; set; }
        public MonthDayEnums ContributionMonthDay { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public Guid ContributionSchemeId { get; set; }
        public virtual ContributionScheme? ContributionScheme { get; set; }
    }
}

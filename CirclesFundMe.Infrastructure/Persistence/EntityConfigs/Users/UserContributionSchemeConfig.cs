namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record UserContributionSchemeConfig : BaseEntityConfig<UserContributionScheme>
    {
        public override void Configure(EntityTypeBuilder<UserContributionScheme> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserContributionSchemes");
            builder.HasKey(x => new { x.UserId, x.ContributionSchemeId });

            builder.Property(x => x.ContributionAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.IncomeAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.CopyOfCurrentAutoBreakdownAtOnboarding)
                .HasMaxLength(2000)
                .IsRequired(false);

            builder.Property(x => x.ContributionWeekDay)
                .HasConversion<EnumToStringConverter<WeekDayEnums>>()
                .HasMaxLength(20);

            builder.Property(x => x.ContributionMonthDay)
                .HasConversion<EnumToStringConverter<MonthDayEnums>>()
                .HasMaxLength(20);

            builder.HasOne(x => x.User)
                .WithOne(x => x.UserContributionScheme)
                .HasForeignKey<UserContributionScheme>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ContributionScheme)
                .WithMany(x => x.UserContributionSchemes)
                .HasForeignKey(x => x.ContributionSchemeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

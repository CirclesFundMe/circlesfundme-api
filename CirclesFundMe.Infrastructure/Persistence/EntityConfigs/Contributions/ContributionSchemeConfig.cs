
namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Contributions
{
    public record ContributionSchemeConfig : BaseEntityConfig<ContributionScheme>
    {
        public override void Configure(EntityTypeBuilder<ContributionScheme> builder)
        {
            base.Configure(builder);
            builder.ToTable("ContributionSchemes");

            builder.HasIndex(x => x.SchemeType)
                .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.SchemeType)
                .HasConversion<EnumToStringConverter<SchemeTypeEnums>>()
                .HasMaxLength(50);

            builder.Property(x => x.ContributionPercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.EligibleLoanMultiple)
                .HasPrecision(18, 2);

            builder.Property(x => x.ServiceCharge)
                .HasPrecision(18, 2);

            builder.Property(x => x.LoanManagementFeePercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.DefaultPenaltyPercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.EquityPercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.LoanTerm)
                .HasPrecision(18, 2);

            builder.Property(x => x.PreLoanServiceChargePercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.PostLoanServiceChargePercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.ExtraEnginePercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.ExtraTyrePercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.InsurancePerAnnumPercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.ProcessingFeePercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.EligibleLoanPercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.DownPaymentPercent)
                .HasPrecision(18, 2);

            builder.Property(x => x.BaseFee)
                .HasPrecision(18, 2);
        }
    }
}

namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Loans
{
    public record LoanApplicationConfig : BaseEntityConfig<LoanApplication>
    {
        public override void Configure(EntityTypeBuilder<LoanApplication> builder)
        {
            base.Configure(builder);
            builder.ToTable("LoanApplications");

            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.Status)
                .HasConversion<EnumToStringConverter<LoanApplicationStatusEnums>>()
                .HasMaxLength(50);

            builder.Property(x => x.RequestedAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.CurrentEligibleAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.ApprovedAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Scheme)
                .HasConversion<EnumToStringConverter<SchemeTypeEnums>>()
                .HasMaxLength(50);

            builder.HasOne(x => x.User)
                .WithMany(x => x.LoanApplications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Loans
{
    public record ApprovedLoanConfig : BaseEntityConfig<ApprovedLoan>
    {
        public override void Configure(EntityTypeBuilder<ApprovedLoan> builder)
        {
            base.Configure(builder);
            builder.ToTable("ApprovedLoans");

            builder.Property(x => x.Status)
                .HasConversion<EnumToStringConverter<ApprovedLoanStatusEnums>>()
                .HasMaxLength(50);

            builder.Property(x => x.ApprovedAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(x => x.LoanApplication)
                .WithOne(x => x.ApprovedLoan)
                .HasForeignKey<ApprovedLoan>(x => x.LoanApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

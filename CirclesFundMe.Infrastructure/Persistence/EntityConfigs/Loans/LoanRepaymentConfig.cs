namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Loans
{
    public record LoanRepaymentConfig : BaseEntityConfig<LoanRepayment>
    {
        public override void Configure(EntityTypeBuilder<LoanRepayment> builder)
        {
            base.Configure(builder);
            builder.ToTable("LoanRepayments");

            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.RepaymentDate)
                .HasColumnType("datetime");

            builder.Property(x => x.Status)
                .HasConversion<EnumToStringConverter<LoanRepaymentStatusEnums>>()
                .HasMaxLength(50);

            builder.Property(x => x.DueDate)
                .HasColumnType("datetime");

            builder.HasOne(x => x.User)
                .WithMany(x => x.LoanRepayments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ApprovedLoan)
                .WithMany(x => x.LoanRepayments)
                .HasForeignKey(x => x.ApprovedLoanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Loans
{
    public record LoanRepaymentConfig : BaseEntityConfig<LoanRepayment>
    {
        public override void Configure(EntityTypeBuilder<LoanRepayment> builder)
        {
            base.Configure(builder);
            builder.ToTable("LoanRepayments");

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.RepaymentDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ApprovedLoan)
                .WithMany(x => x.LoanRepayments)
                .HasForeignKey(x => x.ApprovedLoanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

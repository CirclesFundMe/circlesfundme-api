
namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Finances
{
    public record TransactionConfig : BaseEntityConfig<Transaction>
    {
        public override void Configure(EntityTypeBuilder<Transaction> builder)
        {
            base.Configure(builder);
            builder.ToTable("Transactions");

            builder.HasIndex(p => p.TransactionReference).IsUnique();

            builder.Property(p => p.TransactionReference)
                .HasMaxLength(256);

            builder.Property(p => p.Narration)
                .HasMaxLength(256);

            builder.Property(p => p.TransactionType)
                .HasConversion<EnumToStringConverter<TransactionTypeEnums>>()
                .HasMaxLength(50);

            builder.Property(p => p.BalanceBeforeTransaction)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.BalanceAfterTransaction)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.TransactionDate)
                .HasColumnType("date");

            builder.Property(p => p.TransactionTime)
                .HasColumnType("time");

            builder.HasOne(p => p.Wallet)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

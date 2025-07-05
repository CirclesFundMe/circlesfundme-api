namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Finances
{
    public record WalletConfig : BaseEntityConfig<Wallet>
    {
        public override void Configure(EntityTypeBuilder<Wallet> builder)
        {
            base.Configure(builder);
            builder.ToTable("Wallets");

            builder.HasIndex(x => x.UserId, "IX_Wallets_UserId");

            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.Property(x => x.Balance)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Type)
                .HasConversion<EnumToStringConverter<WalletTypeEnums>>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<EnumToStringConverter<WalletStatusEnums>>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.LastTranDate)
                .HasColumnType("datetime2");

            builder.Property(x => x.NextTranDate)
                .HasColumnType("datetime2");

            builder.Property(x => x.BlockedReason)
                .HasMaxLength(500);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Wallets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record UserWithdrawalSettingConfig : BaseEntityConfig<UserWithdrawalSetting>
    {
        public override void Configure(EntityTypeBuilder<UserWithdrawalSetting> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserWithdrawalSettings");
            builder.HasIndex(x => x.UserId).IsUnique();

            builder.Property(x => x.PaystackRecipientCode)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.AccountNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.AccountName)
                .HasMaxLength(256)
                .IsRequired();

            builder.HasOne(x => x.Bank)
                .WithMany()
                .HasForeignKey(x => x.BankCode)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithOne(x => x.WithdrawalSetting)
                .HasForeignKey<UserWithdrawalSetting>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

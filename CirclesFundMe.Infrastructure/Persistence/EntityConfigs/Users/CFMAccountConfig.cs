
namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record CFMAccountConfig : BaseEntityConfig<CFMAccount>
    {
        public override void Configure(EntityTypeBuilder<CFMAccount> builder)
        {
            base.Configure(builder);
            builder.ToTable("CFMAccounts");

            builder.HasIndex(e => e.Name);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.AccountType)
                .HasConversion<EnumToStringConverter<AccountTypeEnums>>()
                .HasMaxLength(50);

            builder.Property(e => e.AccountStatus)
                .HasConversion<EnumToStringConverter<AccountStatusEnums>>()
                .HasMaxLength(50);

            builder.HasMany(e => e.Users)
                .WithOne(e => e.CFMAccount)
                .HasForeignKey(e => e.CFMAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

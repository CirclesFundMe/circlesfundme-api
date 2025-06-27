namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record UserOtpConfig : BaseEntityConfig<UserOtp>
    {
        public override void Configure(EntityTypeBuilder<UserOtp> builder)
        {
            base.Configure(builder);

            builder.HasKey(e => e.Email);
            builder.HasIndex(e => e.Email).IsUnique();

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.Otp)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}

namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.MiddleName)
                .HasMaxLength(50);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.DateOfBirth)
                .HasColumnType("date");

            builder.Property(u => u.UserType)
                .HasConversion<EnumToStringConverter<UserTypeEnums>>()
                .HasMaxLength(50);

            builder.Property(u => u.RefreshToken)
                .HasMaxLength(256);

            builder.Property(u => u.CreatedBy)
                .HasMaxLength(100);

            builder.Property(u => u.ModifiedBy)
                .HasMaxLength(100);

            builder.Property(u => u.DeletedBy)
                .HasMaxLength(100);

            builder.Property(u => u.OnboardingStatus)
                .HasConversion<EnumToStringConverter<OnboardingStatusEnums>>()
                .HasMaxLength(50);

            builder.Property(u => u.Gender)
                .HasConversion<EnumToStringConverter<GenderEnums>>()
                .HasMaxLength(50)
                .IsRequired(false);
        }
    }
}

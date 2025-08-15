namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record UserContributionConfig : BaseEntityConfig<UserContribution>
    {
        public override void Configure(EntityTypeBuilder<UserContribution> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserContributions");

            builder.HasIndex(x => x.DueDate, "IX_UserContributions_DueDate");
            builder.HasIndex(x => x.UserId, "IX_UserContributions_UserId");

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.AmountIncludingCharges)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Charges)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<EnumToStringConverter<UserContributionStatusEnums>>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.DueDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(x => x.PaidDate)
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserContributions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record UserContributionConfig : BaseEntityConfig<UserContribution>
    {
        public override void Configure(EntityTypeBuilder<UserContribution> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserContributions");

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.AmountIncludingCharges)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserContributions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

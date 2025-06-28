namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record UserContributionSchemeConfig : BaseEntityConfig<UserContributionScheme>
    {
        public override void Configure(EntityTypeBuilder<UserContributionScheme> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserContributionSchemes");
            builder.HasKey(x => new { x.UserId, x.ContributionSchemeId });

            builder.Property(x => x.ContributionAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.IncomeAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithOne(x => x.UserContributionScheme)
                .HasForeignKey<UserContributionScheme>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ContributionScheme)
                .WithMany(x => x.UserContributionSchemes)
                .HasForeignKey(x => x.ContributionSchemeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

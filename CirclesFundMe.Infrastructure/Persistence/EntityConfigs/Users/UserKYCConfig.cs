namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record UserKYCConfig : BaseEntityConfig<UserKYC>
    {
        public override void Configure(EntityTypeBuilder<UserKYC> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserKYC");

            builder.Property(x => x.BVN)
                .HasMaxLength(20);

            builder.Property(x => x.NIN)
                .HasMaxLength(20);

            builder.HasOne(x => x.User)
                .WithOne(x => x.UserKYC)
                .HasForeignKey<UserKYC>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

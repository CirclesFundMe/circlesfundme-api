namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record UserAddressConfig : BaseEntityConfig<UserAddress>
    {
        public override void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserAddresses");

            builder.Property(x => x.FullAddress)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithOne(x => x.UserAddress)
                .HasForeignKey<UserAddress>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

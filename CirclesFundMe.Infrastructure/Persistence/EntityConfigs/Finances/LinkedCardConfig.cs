
namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Finances
{
    public record LinkedCardConfig : BaseEntityConfig<LinkedCard>
    {
        public override void Configure(EntityTypeBuilder<LinkedCard> builder)
        {
            base.Configure(builder);
            builder.ToTable("LinkedCards");
            builder.HasIndex(p => p.UserId)
                .IsUnique();

            builder.Property(p => p.AuthorizationCode)
                .HasMaxLength(256);

            builder.Property(p => p.Last4Digits)
                .HasMaxLength(4);

            builder.Property(p => p.CardType)
                .HasMaxLength(50);

            builder.Property(p => p.ExpiryMonth)
                .HasMaxLength(2);

            builder.Property(p => p.ExpiryYear)
                .HasMaxLength(4);

            builder.Property(p => p.Bin)
                .HasMaxLength(6);

            builder.HasOne(p => p.User)
                .WithOne(x => x.LinkedCard)
                .HasForeignKey<LinkedCard>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

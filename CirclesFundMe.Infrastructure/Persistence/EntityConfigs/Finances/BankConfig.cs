
namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Finances
{
    public record BankConfig : BaseEntityConfig<Bank>
    {
        public override void Configure(EntityTypeBuilder<Bank> builder)
        {
            base.Configure(builder);
            builder.ToTable("Banks");

            builder.HasKey(x => x.Code);
            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}

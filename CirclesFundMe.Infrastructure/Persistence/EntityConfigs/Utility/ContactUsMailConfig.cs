
namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Utility
{
    public record ContactUsMailConfig : BaseEntityConfig<ContactUsMail>
    {
        public override void Configure(EntityTypeBuilder<ContactUsMail> builder)
        {
            base.Configure(builder);
            builder.ToTable("ContactUsMails");

            builder.Property(c => c.FirstName)
                .HasMaxLength(50);

            builder.Property(c => c.LastName)
                .HasMaxLength(50);

            builder.Property(c => c.Email)
                .HasMaxLength(100);

            builder.Property(c => c.Phone)
                .HasMaxLength(20);

            builder.Property(c => c.Title)
                .HasMaxLength(50);

            builder.Property(c => c.Message)
                .HasMaxLength(512);
        }
    }
}


namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.AdminPortal
{
    public record CommunicationConfig : BaseEntityConfig<Communication>
    {
        public override void Configure(EntityTypeBuilder<Communication> builder)
        {
            base.Configure(builder);
            builder.ToTable("Communications");

            builder.HasIndex(c => c.Title, "IX_Communication_Title");

            builder.Property(c => c.Title)
                .HasMaxLength(256);

            builder.Property(c => c.Body)
                .HasMaxLength(1024);

            builder.Property(c => c.Target)
                .HasConversion<EnumToStringConverter<CommunicationTarget>>()
                .HasMaxLength(50);

            builder.Property(c => c.Channel)
                .HasConversion<EnumToStringConverter<CommunicationChannel>>()
                .HasMaxLength(50);

            builder.Property(c => c.Status)
                .HasConversion<EnumToStringConverter<CommunicationStatus>>()
                .HasMaxLength(50);

            builder.Property(c => c.ErrorMessage)
                .HasMaxLength(1024);

            builder.HasMany(c => c.Recipients)
                .WithOne(cr => cr.Communication)
                .HasForeignKey(cr => cr.CommunicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

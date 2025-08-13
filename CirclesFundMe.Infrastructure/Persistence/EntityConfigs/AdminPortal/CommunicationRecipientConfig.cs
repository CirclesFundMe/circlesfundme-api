
namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.AdminPortal
{
    public record CommunicationRecipientConfig : BaseEntityConfig<CommunicationRecipient>
    {
        public override void Configure(EntityTypeBuilder<CommunicationRecipient> builder)
        {
            base.Configure(builder);
            builder.ToTable("CommunicationRecipients");

            builder.HasIndex(cr => cr.UserId, "IX_CommunicationRecipient_UserId");

            builder.Property(cr => cr.Status)
                .HasConversion<EnumToStringConverter<CommunicationRecipientStatus>>()
                .HasMaxLength(50);

            builder.Property(cr => cr.ErrorMessage)
                .HasMaxLength(1024);

            builder.HasOne(cr => cr.User)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cr => cr.Communication)
                .WithMany(c => c.Recipients)
                .HasForeignKey(cr => cr.CommunicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

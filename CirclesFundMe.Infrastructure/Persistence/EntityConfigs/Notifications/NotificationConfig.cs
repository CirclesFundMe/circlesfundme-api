namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Notifications
{
    public record NotificationConfig : BaseEntityConfig<Notification>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            base.Configure(builder);
            builder.ToTable("Notifications");

            builder.HasIndex(n => n.Type, "IX_Notifications_Type");
            builder.HasIndex(n => n.UserId, "IX_Notifications_UserId");

            builder.Property(n => n.SourceName)
                .HasMaxLength(100);

            builder.Property(n => n.SourceAvatar)
                .HasMaxLength(256);

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Content)
                .HasMaxLength(1000);

            builder.Property(n => n.Metadata)
                .HasMaxLength(1000);

            builder.Property(n => n.Type)
                .HasConversion<EnumToStringConverter<NotificationTypeEnums>>()
                .HasMaxLength(50);

            builder.Property(n => n.ObjectId)
                .HasMaxLength(256);

            builder.Property(n => n.ObjectSlug)
                .HasMaxLength(256);

            builder.Property(n => n.ObjectIdentifier)
                .HasMaxLength(256);

            builder.Property(n => n.EmailNotificationSubject)
                .HasMaxLength(256);

            builder.Property(n => n.EmailNotificationErrorMessage)
                .HasMaxLength(1000);

            builder.Property(n => n.PushNotificationErrorMessage)
                .HasMaxLength(1000);

            builder.Property(n => n.SmsNotificationErrorMessage)
                .HasMaxLength(1000);

            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

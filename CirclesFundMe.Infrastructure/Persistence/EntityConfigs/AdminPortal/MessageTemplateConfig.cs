namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.AdminPortal
{
    public record MessageTemplateConfig : BaseEntityConfig<MessageTemplate>
    {
        public override void Configure(EntityTypeBuilder<MessageTemplate> builder)
        {
            base.Configure(builder);
            builder.ToTable("MessageTemplates");

            builder.HasIndex(x => x.Type).IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.Property(x => x.Body)
                .HasMaxLength(int.MaxValue);

            builder.Property(x => x.Channel)
                .HasConversion<EnumToStringConverter<MessageTemplateChannel>>()
                .HasMaxLength(50);

            builder.Property(x => x.Type)
                .HasConversion<EnumToStringConverter<MessageTemplateType>>()
                .HasMaxLength(50);
        }
    }
}

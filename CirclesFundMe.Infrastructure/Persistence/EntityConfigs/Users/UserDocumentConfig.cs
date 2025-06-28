namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record UserDocumentConfig : BaseEntityConfig<UserDocument>
    {
        public override void Configure(EntityTypeBuilder<UserDocument> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserDocuments");

            builder.HasIndex(ud => new { ud.UserId, ud.DocumentType })
                .IsUnique();

            builder.Property(ud => ud.DocumentType)
                .HasConversion<EnumToStringConverter<UserDocumentTypeEnums>>()
                .HasMaxLength(50);

            builder.Property(ud => ud.DocumentUrl)
                .HasMaxLength(500);

            builder.Property(ud => ud.DocumentName)
                .HasMaxLength(256);

            builder.HasOne(ud => ud.User)
                .WithMany(u => u.UserDocuments)
                .HasForeignKey(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

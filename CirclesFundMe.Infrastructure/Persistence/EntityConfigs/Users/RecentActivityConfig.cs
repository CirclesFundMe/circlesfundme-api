namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Users
{
    public record RecentActivityConfig : BaseEntityConfig<RecentActivity>
    {
        public override void Configure(EntityTypeBuilder<RecentActivity> builder)
        {
            base.Configure(builder);
            builder.ToTable("RecentActivities");

            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(x => x.Type)
                .HasConversion<EnumToStringConverter<RecentActivityTypeEnums>>()
                .HasMaxLength(50);

            builder.Property(x => x.Data)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasOne(x => x.User)
                .WithMany(x => x.RecentActivities)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}

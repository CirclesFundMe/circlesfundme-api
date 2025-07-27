
namespace CirclesFundMe.Infrastructure.Persistence.EntityConfigs.Finances
{
    public record PaymentConfig : BaseEntityConfig<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {
            base.Configure(builder);
            builder.ToTable("Payments");
            builder.HasKey(p => p.Reference);
            builder.HasIndex(p => p.Reference)
                .IsUnique();

            builder.Property(p => p.PaymentType)
                .HasConversion<EnumToStringConverter<PaymentTypeEnums>>()
                .HasMaxLength(50);

            builder.Property(p => p.AccessCode)
                .HasMaxLength(256);

            builder.Property(p => p.AuthorizationUrl)
                .HasMaxLength(512);

            builder.Property(p => p.Reference)
                .HasMaxLength(256);

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.ChargeAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Currency)
                .HasMaxLength(5);

            builder.Property(p => p.TransactionDate)
                .HasColumnType("datetime2");

            builder.Property(p => p.Status)
                .HasMaxLength(50);

            builder.Property(p => p.Domain)
                .HasMaxLength(256);

            builder.Property(p => p.GatewayResponse)
                .HasMaxLength(1024);

            builder.Property(p => p.Message)
                .HasMaxLength(1024);

            builder.Property(p => p.Channel)
                .HasMaxLength(50);

            builder.Property(p => p.IpAddress)
                .HasMaxLength(45);

            builder.Property(p => p.AuthorizationCode)
                .HasMaxLength(256);

            builder.Property(p => p.PaymentStatus)
                .HasConversion<EnumToStringConverter<PaymentStatusEnums>>()
                .HasMaxLength(50);
        }
    }
}

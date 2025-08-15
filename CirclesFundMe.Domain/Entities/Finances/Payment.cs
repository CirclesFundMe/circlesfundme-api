namespace CirclesFundMe.Domain.Entities.Finances
{
    public record Payment : BaseEntity
    {
        public PaymentTypeEnums PaymentType { get; set; }
        public string? AccessCode { get; set; }
        public string? AuthorizationUrl { get; set; }
        public string? Reference { get; set; }
        public decimal Amount { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Status { get; set; }
        public string? Domain { get; set; }
        public string? GatewayResponse { get; set; }
        public string? Message { get; set; }
        public string? Channel { get; set; }
        public string? IpAddress { get; set; }
        public string? AuthorizationCode { get; set; }
        public PaymentStatusEnums PaymentStatus { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public void BasicValidate()
        {
            if (!Enum.IsDefined(typeof(PaymentTypeEnums), PaymentType))
                throw new ArgumentException("Invalid payment type specified.", nameof(PaymentType));
        }
    }

    [NotMapped]
    public record PaymentAdmin
    {
        public DateTime Date { get; set; }
        public string? Action { get; set; }
        public decimal Amount { get; set; }
        public decimal Charge { get; set; }
        public PaymentStatusEnums Status { get; set; }
    }
}

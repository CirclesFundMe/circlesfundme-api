namespace CirclesFundMe.Domain.Entities.Finances
{
    public record Payment : BaseEntity
    {
        public string? AccessCode { get; set; }
        public string? AuthorizationUrl { get; set; }
        public string? Reference { get; set; }
        public decimal Amount { get; set; } = 0;
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
    }
}

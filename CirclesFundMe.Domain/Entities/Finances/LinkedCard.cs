namespace CirclesFundMe.Domain.Entities.Finances
{
    public record LinkedCard : BaseEntity
    {
        public string? AuthorizationCode { get; set; }
        public string? Last4Digits { get; set; }
        public string? CardType { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public string? Bin { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}

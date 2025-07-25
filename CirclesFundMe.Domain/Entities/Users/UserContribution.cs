namespace CirclesFundMe.Domain.Entities.Users
{
    public record UserContribution : BaseEntity
    {
        public decimal Amount { get; set; }
        public decimal AmountIncludingCharges { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}

namespace CirclesFundMe.Domain.Entities.Users
{
    public record UserContribution : BaseEntity
    {
        public decimal Amount { get; set; }
        public decimal AmountIncludingCharges { get; set; }
        public decimal Charges { get; set; }
        public UserContributionStatusEnums Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}

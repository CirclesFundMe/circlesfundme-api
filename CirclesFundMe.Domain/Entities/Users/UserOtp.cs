namespace CirclesFundMe.Domain.Entities.Users
{
    public record UserOtp : BaseEntity
    {
        public required string Email { get; set; }
        public required string Otp { get; set; }
        public DateTime GeneratedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
    }
}

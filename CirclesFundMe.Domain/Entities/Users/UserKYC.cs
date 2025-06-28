namespace CirclesFundMe.Domain.Entities.Users
{
    public record UserKYC : BaseEntity
    {
        public string? BVN { get; set; }
        public bool IsBvnVerified { get; set; }
        public string? NIN { get; set; }
        public bool IsNinVerified { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}

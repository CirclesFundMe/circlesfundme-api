namespace CirclesFundMe.Domain.Entities.Users
{
    public record UserAddress : BaseEntity
    {
        public string? FullAddress { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}
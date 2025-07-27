namespace CirclesFundMe.Domain.Entities.Users
{
    public record RecentActivity : BaseEntity
    {
        public string? Title { get; set; }
        public RecentActivityTypeEnums Type { get; set; }
        public string? Data { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}

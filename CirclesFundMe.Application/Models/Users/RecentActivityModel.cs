namespace CirclesFundMe.Application.Models.Users
{
    public record RecentActivityModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public RecentActivityTypeEnums Type { get; set; }
        public string? Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

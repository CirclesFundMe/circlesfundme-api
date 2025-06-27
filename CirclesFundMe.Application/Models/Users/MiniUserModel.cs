namespace CirclesFundMe.Application.Models.Users
{
    public record MiniUserModel
    {
        public required string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}

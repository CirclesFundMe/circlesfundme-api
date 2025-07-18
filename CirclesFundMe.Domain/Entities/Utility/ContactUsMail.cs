namespace CirclesFundMe.Domain.Entities.Utility
{
    public record ContactUsMail : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public bool IsMailSentToAdmin { get; set; }
    }
}

namespace CirclesFundMe.Application.Models.AdminPortal
{
    public record CommunicationModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Target { get; set; }
        public string? Channel { get; set; }
        public string? Status { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int TotalRecipients { get; set; }
    }
}

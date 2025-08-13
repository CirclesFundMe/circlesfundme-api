namespace CirclesFundMe.Domain.Entities.AdminPortal
{
    public record Communication : BaseEntity
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public CommunicationTarget Target { get; set; }
        public CommunicationChannel Channel { get; set; }
        public CommunicationStatus Status { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public int RetryCount { get; set; }

        public virtual ICollection<CommunicationRecipient> Recipients { get; set; } = [];
    }

    [NotMapped]
    public record CommunicationExtension : Communication
    {
        public int TotalRecipients { get; set; }
    }
}

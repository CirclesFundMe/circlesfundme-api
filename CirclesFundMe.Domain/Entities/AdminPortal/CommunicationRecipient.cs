namespace CirclesFundMe.Domain.Entities.AdminPortal
{
    public record CommunicationRecipient : BaseEntity
    {
        public CommunicationRecipientStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public Guid CommunicationId { get; set; }
        public virtual Communication? Communication { get; set; }
    }
}

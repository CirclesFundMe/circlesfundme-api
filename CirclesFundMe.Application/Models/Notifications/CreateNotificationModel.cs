namespace CirclesFundMe.Application.Models.Notifications
{
    public record CreateNotificationModel
    {
        public required string Title { get; set; }
        public string? Metadata { get; set; }
        public NotificationTypeEnums Type { get; set; }
        public string? ObjectId { get; set; }
        public string? UserId { get; set; }
    }
}

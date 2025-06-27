namespace CirclesFundMe.Domain.Entities.Notifications
{
    public record Notification : BaseEntity
    {
        public string? SourceName { get; set; }
        public string? SourceAvatar { get; set; }
        public required string Title { get; set; }
        public string? Content { get; set; }
        public string? Metadata { get; set; }
        public NotificationTypeEnums Type { get; set; }
        public string? ObjectId { get; set; }
        public string? ObjectSlug { get; set; }
        public string? ObjectIdentifier { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool ShouldSendEmailNotification { get; set; }
        public string? EmailNotificationSubject { get; set; }
        public bool IsEmailNotificationSent { get; set; }
        public DateTime? EmailNotificationSentAt { get; set; }
        public int EmailNotificationRetryCount { get; set; }
        public string? EmailNotificationErrorMessage { get; set; }
        public bool ShouldSendPushNotification { get; set; }
        public bool IsPushNotificationSent { get; set; }
        public DateTime? PushNotificationSentAt { get; set; }
        public int PushNotificationRetryCount { get; set; }
        public string? PushNotificationErrorMessage { get; set; }
        public bool ShouldSendSmsNotification { get; set; }
        public bool IsSmsNotificationSent { get; set; }
        public DateTime? SmsNotificationSentAt { get; set; }
        public int SmsNotificationRetryCount { get; set; }
        public string? SmsNotificationErrorMessage { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}

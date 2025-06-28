namespace CirclesFundMe.Application.Models.Notifications
{
    public record NotificationModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public NotificationTypeEnums Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ObjectId { get; set; }
        public object? Data { get; set; }
        public bool IsRead { get; set; }
        public string Colour => DecideNotificationColor(Type);

        private static string DecideNotificationColor(NotificationTypeEnums type)
        {
            return type switch
            {
                NotificationTypeEnums.Contribution => "#00A86B0D",
                NotificationTypeEnums.Withdrawal => "#C608080D",
                _ => "#E6BB1D0D"
            };
        }
    }
}

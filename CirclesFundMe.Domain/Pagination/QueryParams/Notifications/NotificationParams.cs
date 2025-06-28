namespace CirclesFundMe.Domain.Pagination.QueryParams.Notifications
{
    public record NotificationParams : BaseParam
    {
        public NotificationTypeEnums NotificationTypeEnums { get; set; }
        public bool? IsRead { get; set; }
    }
}

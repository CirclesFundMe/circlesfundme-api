namespace CirclesFundMe.Application.CQRS.Queries.Notifications
{
    public record GetNotificationsQuery : IRequest<BaseResponse<PagedList<NotificationModel>>>
    {
        public required NotificationParams NotificationParams { get; init; }
    }
}

namespace CirclesFundMe.Application.CQRS.Commands.Notifications
{
    public record ReadNotificationCommand : IRequest<BaseResponse<bool>>
    {
        public Guid NotificationId { get; init; }
    }
}

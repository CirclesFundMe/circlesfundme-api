namespace CirclesFundMe.API.Controllers.v1
{
    [Authorize]
    public class NotificationsController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpGet]
        [ProducesResponseType<BaseResponse<List<NotificationModel>>>(200)]
        [SwaggerOperation(Summary = "Get Notifications")]
        public async Task<IActionResult> GetNotifications([FromQuery] NotificationParams notificationParams, CancellationToken cancellationToken)
        {
            BaseResponse<PagedList<NotificationModel>> response = await _sender.Send(new GetNotificationsQuery() { NotificationParams = notificationParams }, cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("mark-all-read")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Mark All Notifications as Read")]
        public async Task<IActionResult> MarkAllRead(CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = await _sender.Send(new MarkAllReadCommand(), cancellationToken);
            return HandleResponse(response);
        }

        [HttpPost("{notificationId}/mark-read")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Mark Notification as Read")]
        public async Task<IActionResult> MarkRead(Guid notificationId, CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = await _sender.Send(new ReadNotificationCommand() { NotificationId = notificationId }, cancellationToken);
            return HandleResponse(response);
        }
    }
}

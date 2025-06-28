namespace CirclesFundMe.Application.CQRS.QueryHandlers.Notifications
{
    public class GetNotificationsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        : IRequestHandler<GetNotificationsQuery, BaseResponse<PagedList<NotificationModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<PagedList<NotificationModel>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            PagedList<Notification> notifications = await _unitOfWork.Notifications.GetNotifications(_currentUserService.UserId, request.NotificationParams, cancellationToken);

            List<NotificationModel> notificationModels = notifications.Select(n => new NotificationModel
            {
                Id = n.Id,
                Title = n.Title,
                Type = n.Type,
                CreatedDate = n.CreatedDate,
                ObjectId = n.ObjectId,
                Data = n.Metadata,
                IsRead = n.IsRead
            }).ToList();

            PagedList<NotificationModel> pagedNotificationModels = new(notificationModels, notifications.TotalCount, notifications.CurrentPage, notifications.PageSize);

            return new BaseResponse<PagedList<NotificationModel>>
            {
                Data = pagedNotificationModels,
                MetaData = PagedListHelper<NotificationModel>.GetPaginationInfo(pagedNotificationModels)
            };
        }
    }
}

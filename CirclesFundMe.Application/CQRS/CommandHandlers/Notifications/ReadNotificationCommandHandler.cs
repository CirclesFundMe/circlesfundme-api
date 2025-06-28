namespace CirclesFundMe.Application.CQRS.CommandHandlers.Notifications
{
    public record ReadNotificationCommandHandler(IUnitOfWork UnitOfWork, ICurrentUserService CurrentUserService) : IRequestHandler<ReadNotificationCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = UnitOfWork;
        private readonly ICurrentUserService _currentUserService = CurrentUserService;

        public async Task<BaseResponse<bool>> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
        {
            Notification? notification = await _unitOfWork.Notifications.GetByPrimaryKey(request.NotificationId, cancellationToken);

            if (notification == null)
            {
                return BaseResponse<bool>.NotFound("Notification not found.");
            }

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            notification.UpdateAudit(_currentUserService.UserId);

            _unitOfWork.Notifications.Update(notification);

            bool result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            return new BaseResponse<bool>
            {
                Data = result,
                Message = result ? "Notification marked as read." : "Failed to mark notification as read."
            };
        }
    }
}

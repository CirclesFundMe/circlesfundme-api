namespace CirclesFundMe.Application.CQRS.CommandHandlers.Notifications
{
    public class MarkAllReadCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<MarkAllReadCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<bool>> Handle(MarkAllReadCommand request, CancellationToken cancellationToken)
        {
            return new()
            {
                Data = await _unitOfWork.Notifications.MarkAllRead(_currentUserService.UserId, cancellationToken),
            };
        }
    }
}

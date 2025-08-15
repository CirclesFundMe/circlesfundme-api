namespace CirclesFundMe.Application.CQRS.CommandHandlers.AdminPortal
{
    public class SendKYCReminderCommandHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IQueueService queueService) : IRequestHandler<SendKYCReminderCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IQueueService _queueService = queueService;

        public async Task<BaseResponse<bool>> Handle(SendKYCReminderCommand request, CancellationToken cancellationToken)
        {
            bool userExists = await _userManager.FindByIdAsync(request.UserId) != null;
            if (!userExists)
            {
                return BaseResponse<bool>.NotFound("User not found.");
            }

            bool messageTemplate = await _unitOfWork.MessageTemplates.HasTemplateForType(MessageTemplateType.PendingKYCReminder);
            if (!messageTemplate)
            {
                return BaseResponse<bool>.NotFound("Message template for KYC reminder not found.");
            }

            _queueService.EnqueueFireAndForgetJob<CommunicationJobs>(j => j.ProcessKYCReminderQueue(request.UserId));

            return BaseResponse<bool>.Success(true, "KYC reminder job has been queued successfully.");
        }
    }
}

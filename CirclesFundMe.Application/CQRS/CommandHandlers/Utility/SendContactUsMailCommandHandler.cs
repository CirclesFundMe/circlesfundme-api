namespace CirclesFundMe.Application.CQRS.CommandHandlers.Utility
{
    public class SendContactUsMailCommandHandler(IQueueService queueService) : IRequestHandler<SendContactUsMailCommand, BaseResponse<bool>>
    {
        private readonly IQueueService _queueService = queueService;

        public async Task<BaseResponse<bool>> Handle(SendContactUsMailCommand request, CancellationToken cancellationToken)
        {
            _queueService.EnqueueFireAndForgetJob<CFMJobs>(j => j.SendContactUsMail(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Phone,
                    request.Title,
                    request.Message
                ));

            await Task.CompletedTask;

            return BaseResponse<bool>.Success(true, "We have received your message. Thank you");
        }
    }
}

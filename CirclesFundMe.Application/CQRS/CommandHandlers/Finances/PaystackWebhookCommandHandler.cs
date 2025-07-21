
namespace CirclesFundMe.Application.CQRS.CommandHandlers.Finances
{
    public class PaystackWebhookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<PaystackWebhookCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(PaystackWebhookCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return BaseResponse<bool>.Success(true, "Webhook processed successfully");
        }
    }
}

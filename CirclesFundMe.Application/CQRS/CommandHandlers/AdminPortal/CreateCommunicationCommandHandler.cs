namespace CirclesFundMe.Application.CQRS.CommandHandlers.AdminPortal
{
    public class CreateCommunicationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCommunicationCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(CreateCommunicationCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Communications.AddAsync(new Communication
            {
                Title = request.Title,
                Body = request.Body,
                Target = request.Target,
                Channel = request.Channel,
                Status = CommunicationStatus.Queued,
                ScheduledAt = DateTime.UtcNow
            }, cancellationToken);

            bool isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isSaved)
            {
                return BaseResponse<bool>.BadRequest("Failed to create communication.");
            }

            return BaseResponse<bool>.Success(true, "Communication created successfully.");
        }
    }
}

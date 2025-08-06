using CirclesFundMe.Application.CQRS.Commands.AdminPortal;

namespace CirclesFundMe.Application.CQRS.CommandHandlers.AdminPortal
{
    public class DeleteMessageTemplateCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMessageTemplateCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(DeleteMessageTemplateCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var messageTemplate = await _unitOfWork.MessageTemplates.GetByPrimaryKey(request.Id, cancellationToken);
                if (messageTemplate == null)
                {
                    response.Data = false;
                    response.Message = "Message template not found.";
                    return response;
                }

                _unitOfWork.MessageTemplates.Delete(messageTemplate);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                response.Data = true;
                response.Message = "Message template deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Message = $"An error occurred while deleting the message template: {ex.Message}";
            }

            return response;
        }
    }
}

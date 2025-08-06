using CirclesFundMe.Application.CQRS.Commands.AdminPortal;

namespace CirclesFundMe.Application.CQRS.CommandHandlers.AdminPortal
{
    public class UpdateMessageTemplateCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateMessageTemplateCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(UpdateMessageTemplateCommand request, CancellationToken cancellationToken)
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

                messageTemplate.Name = request.Name ?? messageTemplate.Name;
                messageTemplate.Body = request.Body ?? messageTemplate.Body;
                messageTemplate.Channel = request.Channel;
                messageTemplate.Type = request.Type;

                _unitOfWork.MessageTemplates.Update(messageTemplate);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                response.Data = true;
                response.Message = "Message template updated successfully.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Message = $"An error occurred while updating the message template: {ex.Message}";
            }

            return response;
        }
    }
}

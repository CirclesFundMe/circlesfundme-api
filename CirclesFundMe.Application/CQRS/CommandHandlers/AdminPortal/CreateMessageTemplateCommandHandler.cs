using CirclesFundMe.Application.CQRS.Commands.AdminPortal;
using CirclesFundMe.Domain.Entities.AdminPortal;

namespace CirclesFundMe.Application.CQRS.CommandHandlers.AdminPortal
{
    public class CreateMessageTemplateCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateMessageTemplateCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(CreateMessageTemplateCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var messageTemplate = new MessageTemplate
                {
                    Name = request.Name,
                    Body = request.Body,
                    Channel = request.Channel,
                    Type = request.Type
                };

                await _unitOfWork.MessageTemplates.AddAsync(messageTemplate, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                response.Data = true;
                response.Message = "Message template created successfully.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Message = $"An error occurred while creating the message template: {ex.Message}";
            }

            return response;
        }
    }
}

namespace CirclesFundMe.Application.CQRS.QueryHandlers.AdminPortal
{
    public class GetMessageTemplatesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMessageTemplatesQuery, BaseResponse<List<MessageTemplateModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<List<MessageTemplateModel>>> Handle(GetMessageTemplatesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<MessageTemplateModel>>();

            try
            {
                var messageTemplates = await _unitOfWork.MessageTemplates.GetManyAsync();
                if (messageTemplates == null || !messageTemplates.Any())
                {
                    response.Data = new List<MessageTemplateModel>();
                    response.Message = "No message templates found.";
                    return response;
                }

                response.Data = messageTemplates.Select(mt => new MessageTemplateModel
                {
                    Id = mt.Id,
                    Name = mt.Name,
                    Body = mt.Body,
                    Channel = mt.Channel,
                    Type = mt.Type
                }).ToList();

                response.Message = "Message templates retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Data = new List<MessageTemplateModel>();
                response.Message = $"An error occurred while retrieving message templates: {ex.Message}";
            }

            return response;
        }
    }
}

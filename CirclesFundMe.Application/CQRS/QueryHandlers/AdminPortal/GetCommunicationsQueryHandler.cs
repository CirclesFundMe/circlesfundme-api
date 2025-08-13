
namespace CirclesFundMe.Application.CQRS.QueryHandlers.AdminPortal
{
    public class GetCommunicationsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCommunicationsQuery, BaseResponse<PagedList<CommunicationModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<PagedList<CommunicationModel>>> Handle(GetCommunicationsQuery request, CancellationToken cancellationToken)
        {
            PagedList<CommunicationExtension> communications = await _unitOfWork.Communications.GetCommunications(request.Params, cancellationToken);

            var communicationModels = communications.Select(c => new CommunicationModel
            {
                Id = c.Id,
                Title = c.Title,
                Body = c.Body,
                Target = c.Target.ToString(),
                Channel = c.Channel.ToString(),
                Status = c.Status.ToString(),
                ScheduledAt = c.ScheduledAt,
                TotalRecipients = c.TotalRecipients
            }).ToList();

            return new BaseResponse<PagedList<CommunicationModel>>
            {
                Data = new PagedList<CommunicationModel>(communicationModels, communications.TotalCount, request.Params.PageNumber, request.Params.PageSize),
                MetaData = PagedListHelper<CommunicationExtension>.GetPaginationInfo(communications),
                Message = "Communications retrieved successfully."
            };
        }
    }
}

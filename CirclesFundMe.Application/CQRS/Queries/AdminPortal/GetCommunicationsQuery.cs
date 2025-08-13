namespace CirclesFundMe.Application.CQRS.Queries.AdminPortal
{
    public record GetCommunicationsQuery : IRequest<BaseResponse<PagedList<CommunicationModel>>>
    {
        public required CommunicationParams Params { get; init; }
    }
}

namespace CirclesFundMe.Application.CQRS.Queries.Users
{
    public record GetMyRecentActivityQuery : IRequest<BaseResponse<PagedList<RecentActivityModel>>>
    {
        public required RecentActivityParams Params { get; init; }
    }
}

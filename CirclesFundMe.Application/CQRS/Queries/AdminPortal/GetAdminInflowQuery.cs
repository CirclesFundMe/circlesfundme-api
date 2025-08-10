namespace CirclesFundMe.Application.CQRS.Queries.AdminPortal
{
    public record GetAdminInflowQuery : IRequest<BaseResponse<PagedList<AdminTransactionModel>>>
    {
        public required MinimalParams Params { get; init; }
    }
}

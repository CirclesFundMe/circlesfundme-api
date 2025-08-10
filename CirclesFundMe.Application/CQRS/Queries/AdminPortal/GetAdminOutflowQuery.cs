namespace CirclesFundMe.Application.CQRS.Queries.AdminPortal
{
    public record GetAdminOutflowQuery : IRequest<BaseResponse<PagedList<AdminTransactionModel>>>
    {
        public required MinimalParams Params { get; init; }
    }
}

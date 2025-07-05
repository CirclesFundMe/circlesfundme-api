namespace CirclesFundMe.Application.CQRS.Queries.Finances
{
    public record GetMyWalletsQuery : IRequest<BaseResponse<List<WalletModel>>>;
}

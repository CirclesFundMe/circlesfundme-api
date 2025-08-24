namespace CirclesFundMe.Application.CQRS.Queries.Finances
{
    public record HasActiveLoanQuery : IRequest<BaseResponse<HasActiveLoanModel>>;
}

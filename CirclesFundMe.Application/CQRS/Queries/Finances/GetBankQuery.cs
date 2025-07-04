namespace CirclesFundMe.Application.CQRS.Queries.Finances
{
    public record GetBankQuery : IRequest<BaseResponse<IEnumerable<BankModel>>>;
}

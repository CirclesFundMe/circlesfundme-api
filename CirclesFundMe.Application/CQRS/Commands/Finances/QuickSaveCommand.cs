namespace CirclesFundMe.Application.CQRS.Commands.Finances
{
    public record QuickSaveCommand : IRequest<BaseResponse<InitializeTransactionModel>>;
}

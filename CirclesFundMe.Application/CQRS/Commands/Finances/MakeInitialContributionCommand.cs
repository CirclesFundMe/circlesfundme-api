namespace CirclesFundMe.Application.CQRS.Commands.Finances
{
    public record MakeInitialContributionCommand : IRequest<BaseResponse<InitializeTransactionModel>>
    {
    }
}

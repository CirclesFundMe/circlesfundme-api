namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record UpdateLinkedCardCommand : IRequest<BaseResponse<InitializeTransactionModel>>
    {
        public required string Otp { get; init; }
    }
}

namespace CirclesFundMe.Application.CQRS.Commands.AdminPortal
{
    public record SendKYCReminderCommand : IRequest<BaseResponse<bool>>
    {
        public required string UserId { get; init; }
    }
}

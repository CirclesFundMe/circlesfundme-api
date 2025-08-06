namespace CirclesFundMe.Application.CQRS.Commands.AdminPortal
{
    public record ReactivateUserCommand : IRequest<BaseResponse<bool>>
    {
        public string UserId { get; init; } = string.Empty;
    }
}

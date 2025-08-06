namespace CirclesFundMe.Application.CQRS.Commands.AdminPortal
{
    public record DeactivateUserCommand : IRequest<BaseResponse<bool>>
    {
        public string UserId { get; init; } = string.Empty;
    }
}

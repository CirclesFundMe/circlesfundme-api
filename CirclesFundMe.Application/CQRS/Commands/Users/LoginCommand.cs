namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record LoginCommand : IRequest<BaseResponse<LoginModel>>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}

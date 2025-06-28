namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record SendOTPCommand : IRequest<BaseResponse<bool>>
    {
        public required string Email { get; init; }
    }
}

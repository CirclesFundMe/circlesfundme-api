namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record ForgotPasswordCommand : IRequest<BaseResponse<string>>
    {
        public required string Email { get; set; }
    }
}

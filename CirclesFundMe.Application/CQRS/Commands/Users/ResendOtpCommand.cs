namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record ResendOtpCommand : IRequest<BaseResponse<string>>
    {
        public required string Email { get; set; }
    }

}

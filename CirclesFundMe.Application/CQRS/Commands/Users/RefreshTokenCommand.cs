namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record RefreshTokenCommand : IRequest<BaseResponse<LoginModel>>
    {
        public required string ExpiredToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}

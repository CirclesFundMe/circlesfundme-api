namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record VerifyOtpCommand : IRequest<BaseResponse<bool>>
    {
        public required string Otp { get; set; }
        public required string Email { get; set; }
    }
}

namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record ResetPasswordCommand : IRequest<BaseResponse<string>>
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
        public required string Otp { get; set; }
    }
}

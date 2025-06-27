namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record ChangePasswordCommand : IRequest<BaseResponse<string>>
    {
        public required string OTP { get; init; }
        public required string CurrentPassword { get; init; }
        public required string NewPassword { get; init; }
        public required string ConfirmNewPassword { get; init; }
    }
}

namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record CreateNewUserAccountCommand : IRequest<BaseResponse<bool>>
    {
        public string? Email { get; init; }
        public string? Password { get; init; }
        public string? ConfirmPassword { get; init; }
        public string? OTP { get; init; }
    }
}

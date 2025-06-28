namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record ChangeProfilePictureCommand : IRequest<BaseResponse<bool>>
    {
        public required IFormFile ProfilePicture { get; init; }
    }
}

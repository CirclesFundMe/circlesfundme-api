namespace CirclesFundMe.Application.CQRS.Validators.Users
{
    public class ChangeProfilePictureCommandValidator : AbstractValidator<ChangeProfilePictureCommand>
    {
        public ChangeProfilePictureCommandValidator()
        {
            RuleFor(x => x.ProfilePicture)
                .NotNull()
                .WithMessage("Profile picture is required.")
                .Must(file => file.Length > 0)
                .WithMessage("Profile picture cannot be empty.")
                .Must(file => file.Length <= 5 * 1024 * 1024)
                .WithMessage("Profile picture must not exceed 5 MB.")
                .Must(file => file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                .WithMessage("Profile picture must be a JPEG or PNG image.");
        }
    }
}

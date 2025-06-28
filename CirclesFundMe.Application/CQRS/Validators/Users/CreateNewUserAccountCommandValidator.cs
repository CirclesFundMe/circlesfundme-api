namespace CirclesFundMe.Application.CQRS.Validators.Users
{
    public class CreateNewUserAccountCommandValidator : AbstractValidator<CreateNewUserAccountCommand>
    {
        public CreateNewUserAccountCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.OTP)
                .NotEmpty().WithMessage("OTP is required.");
        }
    }
}

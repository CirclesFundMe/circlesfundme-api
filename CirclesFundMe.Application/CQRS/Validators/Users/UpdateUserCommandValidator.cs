namespace CirclesFundMe.Application.CQRS.Validators.Users
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            When(x => !string.IsNullOrEmpty(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                    .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                    .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.MiddleName), () =>
            {
                RuleFor(x => x.MiddleName)
                    .MaximumLength(50).WithMessage("Middle name cannot exceed 50 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid international format.");
            });

            When(x => x.DateOfBirth.HasValue, () =>
            {
                RuleFor(x => x.DateOfBirth)
                    .Must(date => date!.Value <= DateTime.UtcNow.AddYears(-18))
                    .WithMessage("You must be at least 18 years old to update your profile.");
            });

            When(x => x.ContributionSchemeId.HasValue, () =>
            {
                RuleFor(x => x.ContributionSchemeId)
                .Must(id => id != Guid.Empty).WithMessage("Contribution scheme ID must be a valid GUID and cannot be empty.");

                RuleFor(x => x.IncomeAmount)
                .NotNull().WithMessage("Income amount is required.")
                .GreaterThan(0).WithMessage("Income must be a positive amount.");

                RuleFor(x => x.ContributionAmount)
                .NotNull().WithMessage("Contribution amount is required.")
                .GreaterThan(0).WithMessage("Contribution amount must be a positive amount.");
            });

            When(x => Enum.IsDefined(x.Gender), () =>
            {
                RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Gender is invalid");
            });
        }
    }
}

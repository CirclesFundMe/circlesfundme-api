namespace CirclesFundMe.Application.CQRS.Validators.Users
{
    public class CompleteUserOnboardingCommandValidator : AbstractValidator<CompleteUserOnboardingCommand>
    {
        public CompleteUserOnboardingCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(256).WithMessage("Full name cannot exceed 256 characters.")
                .Must(name => name != null && name.Contains(' ')).WithMessage("Full name must contain at least a space.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(date => date.HasValue && date.Value <= DateTime.UtcNow.AddYears(-18))
                .WithMessage("You must be at least 18 years old to complete onboarding.");

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage($"Gender must be one of the following values: {string.Join(", ", Enum.GetNames<GenderEnums>())}.");

            When(x => !string.IsNullOrEmpty(x.GovernmentIssuedIDUrl), () =>
            {
                RuleFor(x => x.GovernmentIssuedIDUrl)
                    .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    .WithMessage("Government issued ID URL must be a valid URL.");
            });

            When(x => !string.IsNullOrEmpty(x.BVN), () =>
            {
                RuleFor(x => x.BVN)
                    .Matches(@"^\d{11}$").WithMessage("BVN must be exactly 11 digits.");
            });

            When(x => !string.IsNullOrEmpty(x.SelfieUrl), () =>
            {
                RuleFor(x => x.SelfieUrl)
                    .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    .WithMessage("Selfie URL must be a valid URL.");
            });

            When(x => !string.IsNullOrEmpty(x.Address), () =>
            {
                RuleFor(x => x.Address)
                    .MaximumLength(256).WithMessage("Address cannot exceed 256 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.UtilityBillUrl), () =>
            {
                RuleFor(x => x.UtilityBillUrl)
                    .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    .WithMessage("Utility bill URL must be a valid URL.");
            });

            RuleFor(x => x.ContributionSchemeId)
                .NotEmpty().WithMessage("Contribution scheme ID is required.")
                .Must(id => id != Guid.Empty).WithMessage("Contribution scheme ID must be a valid GUID.");
        }
    }
}

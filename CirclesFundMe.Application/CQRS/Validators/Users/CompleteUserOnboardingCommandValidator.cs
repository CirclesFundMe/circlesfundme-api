namespace CirclesFundMe.Application.CQRS.Validators.Users
{
    public class CompleteUserOnboardingCommandValidator : AbstractValidator<CompleteUserOnboardingCommand>
    {
        private readonly AppSettings _appSettings;

        public CompleteUserOnboardingCommandValidator(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;

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

            When(x => x.GovernmentIssuedID != null, () =>
            {
                RuleFor(x => x.GovernmentIssuedID)
                .Must(file => file != null && file.ContentType == "application/pdf")
                .WithMessage("Government issued ID must be a PDF file.")
                .Must(file => file != null && file.Length <= 5 * 1024 * 1024)
                .WithMessage("Government issued ID must not exceed 5MB.");
            });

            When(x => !string.IsNullOrEmpty(x.BVN), () =>
            {
                RuleFor(x => x.BVN)
                .Matches(@"^\d{11}$").WithMessage("BVN must be exactly 11 digits.");
            });

            When(x => x.Selfie != null, () =>
            {
                RuleFor(x => x.Selfie)
                .Must(file => file != null && (file.ContentType == "image/png" || file.ContentType == "image/jpeg"))
                .WithMessage("Selfie must be a PNG or JPEG file.")
                .Must(file => file != null && file.Length <= 5 * 1024 * 1024)
                .WithMessage("Selfie must not exceed 5MB.");
            });

            When(x => !string.IsNullOrEmpty(x.Address), () =>
            {
                RuleFor(x => x.Address)
                .MaximumLength(256).WithMessage("Address cannot exceed 256 characters.");
            });

            When(x => x.UtilityBill != null, () =>
            {
                RuleFor(x => x.UtilityBill)
                .Must(file => file != null && (file.ContentType == "application/pdf" || file.ContentType == "image/png" || file.ContentType == "image/jpeg"))
                .WithMessage("Utility bill must be a PDF, PNG, or JPEG file.")
                .Must(file => file != null && file.Length <= 5 * 1024 * 1024)
                .WithMessage("Utility bill must not exceed 5MB.");
            });

            RuleFor(x => x.ContributionSchemeId)
                .NotEmpty().WithMessage("Contribution scheme ID is required.")
                .Must(id => id != Guid.Empty).WithMessage("Contribution scheme ID must be a valid GUID.");

            //RuleFor(x => x.Income)
            //    .GreaterThan(0).WithMessage("Income must be a positive amount.");

            //RuleFor(x => x.ContributionAmount)
            //    .GreaterThan(0).WithMessage("Contribution amount must be a positive amount.")
            //    .LessThanOrEqualTo(x => x.Income * (decimal)_appSettings.IncomeToContributionPercentage)
            //    .WithMessage($"Contribution amount must not exceed {_appSettings.IncomeToContributionPercentage * 100}% of income.");
        }
    }
}

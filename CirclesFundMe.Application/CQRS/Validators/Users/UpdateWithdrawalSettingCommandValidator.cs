namespace CirclesFundMe.Application.CQRS.Validators.Users
{
    public class UpdateWithdrawalSettingCommandValidator : AbstractValidator<UpdateWithdrawalSettingCommand>
    {
        public UpdateWithdrawalSettingCommandValidator()
        {
            RuleFor(x => x.WithdrawalSettingId)
                .NotEmpty().WithMessage("Withdrawal setting ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Withdrawal setting ID cannot be empty.");

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("Account number is required.")
                .Length(10, 20).WithMessage("Account number must be between 10 to 20 digits.");

            RuleFor(x => x.BankCode)
                .NotEmpty().WithMessage("Bank code is required.")
                .Length(3, 7).WithMessage("Bank code must be between 3 to 7 characters.");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP is required.");
        }
    }
}

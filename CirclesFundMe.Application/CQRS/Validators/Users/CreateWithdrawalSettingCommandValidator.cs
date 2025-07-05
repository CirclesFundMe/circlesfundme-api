namespace CirclesFundMe.Application.CQRS.Validators.Users
{
    public class CreateWithdrawalSettingCommandValidator : AbstractValidator<CreateWithdrawalSettingCommand>
    {
        public CreateWithdrawalSettingCommandValidator()
        {
            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("Account number is required.")
                .Length(10, 20).WithMessage("Account number must be between 10 and 20 characters long.");

            RuleFor(x => x.BankCode)
                .NotEmpty().WithMessage("Bank code is required.")
                .Length(3, 10).WithMessage("Bank code must be between 3 and 10 characters long.");
        }
    }
}

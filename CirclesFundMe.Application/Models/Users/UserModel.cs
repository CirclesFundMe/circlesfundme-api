namespace CirclesFundMe.Application.Models.Users
{
    public class UserModel
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ContributionSchemeMiniModel? ContributionScheme { get; set; }
        public WithdrawalSettingModel? WithdrawalSetting { get; set; }
        public decimal? ContributionAmount { get; set; }
        public decimal? IncomeAmount { get; set; }
        public string? Email { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? OnboardingStatus { get; set; }
        public string? Gender { get; set; }
        public bool IsCardLinked { get; set; }
        public string? InstallmentDesc { get; set; }

        public bool? AllowPushNotifications { get; set; }
        public bool? AllowEmailNotifications { get; set; }
        public bool IsPaymentSetupComplete { get; set; }
        public MyAutoLoanDetail? AutoLoanDetail { get; set; }
    }

    public record MyAutoLoanDetail
    {
        public decimal CostOfVehicle { get; set; }
        public decimal PreLoanContributionAmount { get; set; }
        public decimal PostLoanWeeklyContribution { get; set; }
    }
}

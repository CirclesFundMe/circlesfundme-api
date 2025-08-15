namespace CirclesFundMe.Domain.Entities.Users
{
    public class AppUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserTypeEnums UserType { get; set; }
        public string? TimeZone { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? CoverPictureUrl { get; set; }
        public OnboardingStatusEnums OnboardingStatus { get; set; }
        public GenderEnums? Gender { get; set; }

        public bool AgreedToTerms { get; set; }
        public string? RefreshToken { get; set; }
        public bool AllowPushNotifications { get; set; }
        public bool AllowEmailNotifications { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public Guid CFMAccountId { get; set; }
        public virtual CFMAccount? CFMAccount { get; set; }

        public virtual UserAddress? UserAddress { get; set; }
        public virtual UserWithdrawalSetting? WithdrawalSetting { get; set; }
        public virtual UserContributionScheme? UserContributionScheme { get; set; }
        public virtual UserKYC? UserKYC { get; set; }
        public virtual LinkedCard? LinkedCard { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; } = [];
        public virtual ICollection<UserDocument> UserDocuments { get; set; } = [];
        public virtual ICollection<Wallet> Wallets { get; set; } = [];
        public virtual ICollection<UserContribution> UserContributions { get; set; } = [];
        public virtual ICollection<RecentActivity> RecentActivities { get; set; } = [];
        public virtual ICollection<LoanApplication> LoanApplications { get; set; } = [];
        public virtual ICollection<ApprovedLoan> ApprovedLoans { get; set; } = [];
        public virtual ICollection<LoanRepayment> LoanRepayments { get; set; } = [];

        public void UpdateAuditFields(string userId)
        {
            ModifiedBy = userId;
            ModifiedDate = DateTime.UtcNow;
        }
    }

    [NotMapped]
    public class AppUserExtension : AppUser
    {
        public bool IsPaymentSetupComplete { get; set; }
        public bool IsCardLinked { get; set; }
        public int PaidContributionsCount { get; set; }
        public int TotalContributionsCount { get; set; }
        public int PaidLoanRepaymentsCount { get; set; }
        public int TotalLoanRepaymentsCount { get; set; }
        public bool IsEligibleForLoan { get; set; }
    }

    [NotMapped]
    public class AppUserAdmin
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public DateTime? DateJoined { get; set; }
        public SchemeTypeEnums SchemeType { get; set; }
        public string? Scheme { get; set; }
        public string? CopyOfCurrentBreakdownAtOnboarding { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal TotalRepaidAmount { get; set; }
        public bool IsDeleted { get; set; }
    }
}

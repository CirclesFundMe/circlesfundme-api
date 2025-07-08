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
        public bool IsPaymentSetupComplete { get; set; }

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
    }

    public class AppUserExtension : AppUser
    {
    }
}

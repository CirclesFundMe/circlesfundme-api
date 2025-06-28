namespace CirclesFundMe.Application.Models.Users
{
    public class UserModel
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public UserTypeEnums UserType { get; set; }
        public string? UserTypeName => UserType.ToString();
        public string? ProfilePictureUrl { get; set; }
        public string? OnboardingStatus { get; set; }

        public bool AllowPushNotifications { get; set; }
        public bool AllowBiometricLogin { get; set; }
    }
}

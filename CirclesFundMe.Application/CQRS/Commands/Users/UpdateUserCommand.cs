namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record UpdateUserCommand : IRequest<BaseResponse<bool>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? ContributionSchemeId { get; set; }
        public decimal? ContributionAmount { get; set; }
        public decimal? IncomeAmount { get; set; }
        public bool? AllowPushNotifications { get; set; }
        public bool? AllowEmailNotifications { get; set; }
        public GenderEnums Gender { get; set; }
    }
}

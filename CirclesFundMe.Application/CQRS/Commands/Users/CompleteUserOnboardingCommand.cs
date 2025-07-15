namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record CompleteUserOnboardingCommand : IRequest<BaseResponse<bool>>
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderEnums Gender { get; set; }
        public IFormFile? GovernmentIssuedID { get; set; }
        public string? BVN { get; set; }
        public IFormFile? Selfie { get; set; }
        public string? Address { get; set; }
        public IFormFile? UtilityBill { get; set; }
        public Guid ContributionSchemeId { get; set; }
        public decimal Income { get; set; }
        public decimal ContributionAmount { get; set; }
        public decimal CostOfVehicle { get; set; }
        public WeekDayEnums WeekDay { get; set; }
        public MonthDayEnums MonthDay { get; set; }
    }
}

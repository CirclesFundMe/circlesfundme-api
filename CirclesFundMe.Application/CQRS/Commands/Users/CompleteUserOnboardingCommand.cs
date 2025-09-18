namespace CirclesFundMe.Application.CQRS.Commands.Users
{
    public record CompleteUserOnboardingCommand : IRequest<BaseResponse<bool>>
    {
        public string? GovernmentIssuedIDUrl { get; set; }
        public string? BVN { get; set; }
        public string? SelfieUrl { get; set; }
        public string? Address { get; set; }
        public string? UtilityBillUrl { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderEnums Gender { get; set; }
        public Guid ContributionSchemeId { get; set; }
        public decimal? Income { get; set; }
        public decimal ContributionAmount { get; set; }
        public int? WeekDay { get; set; }
        public int? MonthDay { get; set; }
        public decimal? CostOfVehicle { get; set; }
    }
}

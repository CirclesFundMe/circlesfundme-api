namespace CirclesFundMe.Application.Models.AdminPortal
{
    public record AdminUserModel
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public DateTime? DateJoined { get; set; }
        public string? Scheme { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal EligibleLoan { get; set; }
        public decimal TotalRepaidAmount { get; set; }
    }
}

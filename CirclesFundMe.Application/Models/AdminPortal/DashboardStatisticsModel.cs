namespace CirclesFundMe.Application.Models.AdminPortal
{
    public record DashboardStatisticsModel
    {
        public int TotalPendingKYCs { get; set; }
        public int TotalActiveLoans { get; set; }
        public int TotalOverduePayments { get; set; }
        public int TotalUsers { get; set; }
    }
}

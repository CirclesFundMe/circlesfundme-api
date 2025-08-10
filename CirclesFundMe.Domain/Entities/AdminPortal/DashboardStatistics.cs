namespace CirclesFundMe.Domain.Entities.AdminPortal
{
    [NotMapped]
    public record DashboardStatistics
    {
        public int TotalPendingKYCs { get; set; }
        public int TotalActiveLoans { get; set; }
        public int TotalOverduePayments { get; set; }
        public int TotalUsers { get; set; }
    }
}

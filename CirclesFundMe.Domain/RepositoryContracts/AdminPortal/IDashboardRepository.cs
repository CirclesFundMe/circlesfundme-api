namespace CirclesFundMe.Domain.RepositoryContracts.AdminPortal
{
    public interface IDashboardRepository
    {
        Task<DashboardStatistics> GetDashboardStatisticsAsync(CancellationToken cancellationToken);
    }
}

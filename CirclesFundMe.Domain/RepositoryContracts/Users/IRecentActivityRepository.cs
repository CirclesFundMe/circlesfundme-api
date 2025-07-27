namespace CirclesFundMe.Domain.RepositoryContracts.Users
{
    public interface IRecentActivityRepository : IRepositoryBase<RecentActivity>
    {
        Task<PagedList<RecentActivity>> GetMyRecentActivities(string userId, RecentActivityParams @params, CancellationToken cancellationToken);
    }
}

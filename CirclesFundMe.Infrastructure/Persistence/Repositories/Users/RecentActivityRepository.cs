namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class RecentActivityRepository(DbSet<RecentActivity> recentActivities) : RepositoryBase<RecentActivity>(recentActivities), IRecentActivityRepository
    {
        private readonly DbSet<RecentActivity> _recentActivities = recentActivities;

        public async Task<PagedList<RecentActivity>> GetMyRecentActivities(string userId, RecentActivityParams @params, CancellationToken cancellationToken)
        {
            DateTime thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            IQueryable<RecentActivity> query = _recentActivities
                .Where(x => x.UserId == userId && x.CreatedDate >= thirtyDaysAgo)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(@params.SearchKey))
            {
                query = query.Where(x => x.Title != null && x.Title.Contains(@params.SearchKey));
            }

            if (Enum.IsDefined(@params.Type) && @params.Type != RecentActivityTypeEnums.None)
            {
                query = query.Where(x => x.Type == @params.Type);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            List<RecentActivity> items = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((@params.PageNumber - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .Select(x => new RecentActivity
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type,
                    Data = x.Data,
                    UserId = x.UserId,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync(cancellationToken);

            return PagedList<RecentActivity>.ToPagedList(items, totalCount, @params.PageNumber, @params.PageSize);
        }
    }
}

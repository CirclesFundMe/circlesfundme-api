namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserContributionRepository(DbSet<UserContribution> contributions) : RepositoryBase<UserContribution>(contributions), IUserContributionRepository
    {
        private readonly DbSet<UserContribution> _contributions = contributions;
        public async Task<decimal> CumulativeUserContribution(string userId, CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return 0;
            }

            return await _contributions
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Amount, cancellationToken: cancellation);
        }
    }
}

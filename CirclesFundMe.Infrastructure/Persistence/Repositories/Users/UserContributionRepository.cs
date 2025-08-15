namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserContributionRepository(SqlDbContext context) : RepositoryBase<UserContribution>(context.UserContributions), IUserContributionRepository
    {
        private readonly DbSet<UserContribution> _contributions = context.UserContributions;
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

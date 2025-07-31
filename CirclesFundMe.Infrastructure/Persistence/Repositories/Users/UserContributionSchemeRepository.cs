namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserContributionSchemeRepository(DbSet<UserContributionScheme> userContributionSchemes) : RepositoryBase<UserContributionScheme>(userContributionSchemes), IUserContributionSchemeRepository
    {
        private readonly DbSet<UserContributionScheme> _userContributionSchemes = userContributionSchemes;

        public async Task<UserContributionScheme?> ViewMyEligibleLoan(string userId, CancellationToken cancellation)
        {
            return await _userContributionSchemes
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new UserContributionScheme
                {
                    Id = x.Id,
                    CopyOfCurrentBreakdownAtOnboarding = x.CopyOfCurrentBreakdownAtOnboarding,
                    ContributionScheme = x.ContributionScheme != null ? new ContributionScheme
                    {
                        Id = x.ContributionScheme.Id,
                        Name = x.ContributionScheme.Name,
                        SchemeType = x.ContributionScheme.SchemeType,
                    } : null,
                })
                .FirstOrDefaultAsync(cancellation);
        }
    }
}

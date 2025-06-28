namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Contributions
{
    public class ContributionSchemeRepository(DbSet<ContributionScheme> contributionSchemes)
        : RepositoryBase<ContributionScheme>(contributionSchemes), IContributionSchemeRepository
    {
        private readonly DbSet<ContributionScheme> _contributionSchemes = contributionSchemes;

        public async Task<List<ContributionScheme>> GetContributionSchemes(CancellationToken cancellationToken)
        {
            return await _contributionSchemes
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ContributionScheme>> GetContributionSchemesMini(CancellationToken cancellationTokens)
        {
            return await _contributionSchemes
                .AsNoTracking()
                .Select(cs => new ContributionScheme
                {
                    Id = cs.Id,
                    Name = cs.Name,
                    SchemeType = cs.SchemeType
                })
                .ToListAsync(cancellationTokens);
        }
    }
}

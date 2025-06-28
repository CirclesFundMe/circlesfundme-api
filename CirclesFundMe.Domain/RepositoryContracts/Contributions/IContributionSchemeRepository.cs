namespace CirclesFundMe.Domain.RepositoryContracts.Contributions
{
    public interface IContributionSchemeRepository : IRepositoryBase<ContributionScheme>
    {
        Task<List<ContributionScheme>> GetContributionSchemesAsync(CancellationToken cancellationTokens);
    }
}

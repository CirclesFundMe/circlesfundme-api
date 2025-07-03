namespace CirclesFundMe.Domain.RepositoryContracts.Contributions
{
    public interface IContributionSchemeRepository : IRepositoryBase<ContributionScheme>
    {
        Task<List<ContributionScheme>> GetContributionSchemesMini(CancellationToken cancellationTokens);
        Task<List<ContributionScheme>> GetContributionSchemes(CancellationToken cancellationToken);
        Task<AutoFinanceBreakdown?> GetAutoFinanceBreakdown(decimal costOfVehicle, CancellationToken cancellation);
    }
}

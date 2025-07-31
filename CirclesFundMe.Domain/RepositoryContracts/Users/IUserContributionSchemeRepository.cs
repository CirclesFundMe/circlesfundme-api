namespace CirclesFundMe.Domain.RepositoryContracts.Users
{
    public interface IUserContributionSchemeRepository : IRepositoryBase<UserContributionScheme>
    {
        Task<UserContributionScheme?> ViewMyEligibleLoan(string userId, CancellationToken cancellation);
    }
}

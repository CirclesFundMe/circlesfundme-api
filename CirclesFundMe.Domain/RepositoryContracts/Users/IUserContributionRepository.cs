namespace CirclesFundMe.Domain.RepositoryContracts.Users
{
    public interface IUserContributionRepository : IRepositoryBase<UserContribution>
    {
        Task<decimal> CumulativeUserContribution(string userId, CancellationToken cancellation);
        Task<UserContribution?> GetNextContributionForPayment(string userId, CancellationToken cancellation);
    }
}

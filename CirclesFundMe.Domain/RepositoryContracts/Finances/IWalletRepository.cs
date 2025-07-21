namespace CirclesFundMe.Domain.RepositoryContracts.Finances
{
    public interface IWalletRepository : IRepositoryBase<Wallet>
    {
        Task<IEnumerable<Wallet>> GetMyWallets(string userId, CancellationToken cancellationToken = default);
        Task<bool> HasInsufficientFundOnContributionWallet(string userId, decimal amount, CancellationToken cancellationToken = default);
        Task<Wallet?> GetUserContributionWallet(string userId, CancellationToken cancellationToken = default);
    }
}

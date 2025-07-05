namespace CirclesFundMe.Domain.RepositoryContracts.Finances
{
    public interface IWalletRepository : IRepositoryBase<Wallet>
    {
        Task<IEnumerable<Wallet>> GetMyWallets(string userId, CancellationToken cancellationToken = default);
    }
}

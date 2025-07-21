namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Finances
{
    public class WalletRepository(DbSet<Wallet> wallets) : RepositoryBase<Wallet>(wallets), IWalletRepository
    {
        private readonly DbSet<Wallet> _wallets = wallets;

        public async Task<IEnumerable<Wallet>> GetMyWallets(string userId, CancellationToken cancellationToken = default)
        {
            return await _wallets
                .AsNoTracking()
                .Where(wallet => wallet.UserId == userId)
                .Select(w => new Wallet
                {
                    Balance = w.Balance,
                    Type = w.Type,
                    Status = w.Status,
                    LastTranDate = w.LastTranDate,
                    NextTranDate = w.NextTranDate,
                    User = w.User != null ? new AppUser
                    {
                        FirstName = w.User.FirstName,
                        LastName = w.User.LastName,
                        UserContributionScheme = w.User.UserContributionScheme != null ? new UserContributionScheme
                        {
                            ContributionScheme = w.User.UserContributionScheme.ContributionScheme != null ? new ContributionScheme
                            {
                                Name = w.User.UserContributionScheme.ContributionScheme.Name,
                                SchemeType = w.User.UserContributionScheme.ContributionScheme.SchemeType
                            } : null,
                        } : null,
                    } : null,
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<Wallet?> GetUserContributionWallet(string userId, CancellationToken cancellationToken = default)
        {
            return await _wallets
                .AsNoTracking()
                .Where(wallet => wallet.UserId == userId && wallet.Type == WalletTypeEnums.Contribution)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> HasInsufficientFundOnContributionWallet(string userId, decimal amount, CancellationToken cancellationToken = default)
        {
            return await _wallets
                .AsNoTracking()
                .Where(wallet => wallet.UserId == userId && wallet.Type == WalletTypeEnums.Contribution)
                .AnyAsync(wallet => wallet.Balance < amount, cancellationToken);
        }
    }
}

namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Common
{
    public class UnitOfWork(SqlDbContext sqlDbContext) : IUnitOfWork
    {
        // Fields
        private readonly SqlDbContext _sqlDbContext = sqlDbContext;
        private IDbContextTransaction? _transaction;

        #region Users
        private IUserRepository? _users;
        private ICFMAccountRepository? _accounts;
        private IUserOtpRepository? _userOtps;
        private IUserDocumentRepository? _userDocuments;
        private IUserAddressRepository? _userAddresses;
        private IUserContributionSchemeRepository? _userContributionSchemes;
        private IUserKYCRepository? _userKYC;
        private IUserWithdrawalSettingRepository? _userWithdrawalSettings;
        #endregion

        #region Contributions
        private IContributionSchemeRepository? _contributionSchemes;
        #endregion

        #region Notifications
        private INotificationRepository? _notifications;
        #endregion

        #region Finances
        private IBankRepository? _banks;
        private IWalletRepository? _wallets;
        #endregion

        // Repositories
        #region Users
        public IUserRepository Users => _users ??= new UserRepository(_sqlDbContext);
        public ICFMAccountRepository Accounts => _accounts ??= new CFMAccountRepository(_sqlDbContext.CFMAccounts);
        public IUserOtpRepository UserOtps => _userOtps ??= new UserOtpRepository(_sqlDbContext.UserOtps);
        public IUserDocumentRepository UserDocuments => _userDocuments ??= new UserDocumentRepository(_sqlDbContext.UserDocuments);
        public IUserAddressRepository UserAddresses => _userAddresses ??= new UserAddressRepository(_sqlDbContext.UserAddresses);
        public IUserContributionSchemeRepository UserContributionSchemes => _userContributionSchemes ??= new UserContributionSchemeRepository(_sqlDbContext.UserContributionSchemes);
        public IUserKYCRepository UserKYC => _userKYC ??= new UserKYCRepository(_sqlDbContext.UserKYCs);
        public IUserWithdrawalSettingRepository UserWithdrawalSettings => _userWithdrawalSettings ??= new UserWithdrawalSettingRepository(_sqlDbContext.UserWithdrawalSettings);
        #endregion

        #region Contributions
        public IContributionSchemeRepository ContributionSchemes => _contributionSchemes ??= new ContributionSchemeRepository(_sqlDbContext.ContributionSchemes);
        #endregion

        #region Notifications
        public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_sqlDbContext.Notifications, _sqlDbContext);
        #endregion

        #region Finances
        public IBankRepository Banks => _banks ??= new BankRepository(_sqlDbContext.Banks);
        public IWalletRepository Wallets => _wallets ??= new WalletRepository(_sqlDbContext.Wallets);
        #endregion

        #region Required Methods
        public async Task BeginTransactionAsync(CancellationToken cancellation)
        {
            _transaction = await _sqlDbContext.Database.BeginTransactionAsync(cancellation);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _sqlDbContext.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            _sqlDbContext.Dispose();
            _transaction?.Dispose();
        }
        #endregion
    }
}

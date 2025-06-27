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
        #endregion

        // Repositories
        #region Users
        public IUserRepository Users => _users ??= new UserRepository(_sqlDbContext);
        public ICFMAccountRepository Accounts => _accounts ??= new CFMAccountRepository(_sqlDbContext.CFMAccounts);
        public IUserOtpRepository UserOtps => _userOtps ??= new UserOtpRepository(_sqlDbContext.UserOtps);
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

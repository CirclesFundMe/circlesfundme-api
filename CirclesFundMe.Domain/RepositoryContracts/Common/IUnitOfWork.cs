namespace CirclesFundMe.Domain.RepositoryContracts.Common
{
    public interface IUnitOfWork : IDisposable
    {
        #region Users
        IUserRepository Users { get; }
        IUserOtpRepository UserOtps { get; }
        #endregion

        #region Required Contracts
        Task BeginTransactionAsync(CancellationToken cancellationToken);
        Task CommitTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}

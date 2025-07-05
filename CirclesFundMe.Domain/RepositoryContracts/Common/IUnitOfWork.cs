namespace CirclesFundMe.Domain.RepositoryContracts.Common
{
    public interface IUnitOfWork : IDisposable
    {
        #region Users
        ICFMAccountRepository Accounts { get; }
        IUserRepository Users { get; }
        IUserOtpRepository UserOtps { get; }
        IUserDocumentRepository UserDocuments { get; }
        IUserAddressRepository UserAddresses { get; }
        IUserKYCRepository UserKYC { get; }
        IUserContributionSchemeRepository UserContributionSchemes { get; }
        IUserWithdrawalSettingRepository UserWithdrawalSettings { get; }
        #endregion

        #region Contributions
        IContributionSchemeRepository ContributionSchemes { get; }
        #endregion

        #region Notifications
        INotificationRepository Notifications { get; }
        #endregion

        #region Finances
        IBankRepository Banks { get; }
        #endregion

        #region Required Contracts
        Task BeginTransactionAsync(CancellationToken cancellationToken);
        Task CommitTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}

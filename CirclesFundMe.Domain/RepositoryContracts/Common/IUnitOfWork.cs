namespace CirclesFundMe.Domain.RepositoryContracts.Common
{
    public interface IUnitOfWork : IAsyncDisposable
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
        IUserContributionRepository UserContributions { get; }
        IRecentActivityRepository RecentActivities { get; }
        #endregion

        #region Contributions
        IContributionSchemeRepository ContributionSchemes { get; }
        #endregion

        #region Notifications
        INotificationRepository Notifications { get; }
        #endregion

        #region Finances
        IBankRepository Banks { get; }
        IWalletRepository Wallets { get; }
        ITransactionRepository Transactions { get; }
        IPaymentRepository Payments { get; }
        ILinkedCardRepository LinkedCards { get; }
        #endregion

        #region Utility
        IContactUsMailRepository ContactUsMails { get; }
        #endregion

        #region Loans
        ILoanApplicationRepository LoanApplications { get; }
        IApprovedLoanRepository ApprovedLoans { get; }
        ILoanRepaymentRepository LoanRepayments { get; }
        #endregion

        #region Admin Portal
        IUserManagementRepository UserManagement { get; }
        IMessageTemplateRepository MessageTemplates { get; }
        IDashboardRepository Dashboard { get; }
        ICommunicationRepository Communications { get; }
        ICommunicationRecipientRepository CommunicationRecipients { get; }
        #endregion

        #region Required Contracts
        Task BeginTransactionAsync(CancellationToken cancellationToken);
        Task CommitTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}

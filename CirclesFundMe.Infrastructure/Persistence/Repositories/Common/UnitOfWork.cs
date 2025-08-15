namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Common
{
    public class UnitOfWork(SqlDbContext sqlDbContext, IServiceProvider sp) : IUnitOfWork
    {
        // Fields
        private readonly SqlDbContext _sqlDbContext = sqlDbContext;
        private IDbContextTransaction? _transaction;

        #region Users
        private readonly Lazy<IUserRepository> _users = NewLazy<IUserRepository>(sp);
        private readonly Lazy<ICFMAccountRepository> _accounts = NewLazy<ICFMAccountRepository>(sp);
        private readonly Lazy<IUserOtpRepository> _userOtps = NewLazy<IUserOtpRepository>(sp);
        private readonly Lazy<IUserDocumentRepository> _userDocuments = NewLazy<IUserDocumentRepository>(sp);
        private readonly Lazy<IUserAddressRepository> _userAddresses = NewLazy<IUserAddressRepository>(sp);
        private readonly Lazy<IUserContributionSchemeRepository> _userContributionSchemes = NewLazy<IUserContributionSchemeRepository>(sp);
        private readonly Lazy<IUserKYCRepository> _userKYC = NewLazy<IUserKYCRepository>(sp);
        private readonly Lazy<IUserWithdrawalSettingRepository> _userWithdrawalSettings = NewLazy<IUserWithdrawalSettingRepository>(sp);
        private readonly Lazy<IUserContributionRepository> _userContributions = NewLazy<IUserContributionRepository>(sp);
        private readonly Lazy<IRecentActivityRepository> _recentActivities = NewLazy<IRecentActivityRepository>(sp);
        #endregion

        #region Contributions
        private readonly Lazy<IContributionSchemeRepository> _contributionSchemes = NewLazy<IContributionSchemeRepository>(sp);
        #endregion

        #region Notifications
        private readonly Lazy<INotificationRepository> _notifications = NewLazy<INotificationRepository>(sp);
        #endregion

        #region Finances
        private readonly Lazy<IBankRepository> _banks = NewLazy<IBankRepository>(sp);
        private readonly Lazy<IWalletRepository> _wallets = NewLazy<IWalletRepository>(sp);
        private readonly Lazy<ITransactionRepository> _transactions = NewLazy<ITransactionRepository>(sp);
        private readonly Lazy<IPaymentRepository> _payments = NewLazy<IPaymentRepository>(sp);
        private readonly Lazy<ILinkedCardRepository> _linkedCards = NewLazy<ILinkedCardRepository>(sp);
        #endregion

        #region Utility
        private readonly Lazy<IContactUsMailRepository> _contactUsMails = NewLazy<IContactUsMailRepository>(sp);
        #endregion

        #region Loans
        private readonly Lazy<ILoanApplicationRepository> _loanApplications = NewLazy<ILoanApplicationRepository>(sp);
        private readonly Lazy<IApprovedLoanRepository> _approvedLoans = NewLazy<IApprovedLoanRepository>(sp);
        private readonly Lazy<ILoanRepaymentRepository> _loanRepayments = NewLazy<ILoanRepaymentRepository>(sp);
        #endregion

        #region Admin Portal
        private readonly Lazy<IUserManagementRepository> _userManagement = NewLazy<IUserManagementRepository>(sp);
        private readonly Lazy<IMessageTemplateRepository> _messageTemplates = NewLazy<IMessageTemplateRepository>(sp);
        private readonly Lazy<IDashboardRepository> _dashboard = NewLazy<IDashboardRepository>(sp);
        private readonly Lazy<ICommunicationRepository> _communications = NewLazy<ICommunicationRepository>(sp);
        private readonly Lazy<ICommunicationRecipientRepository> _communicationRecipients = NewLazy<ICommunicationRecipientRepository>(sp);
        #endregion

        // Repositories
        #region Users
        public IUserRepository Users => _users.Value;
        public ICFMAccountRepository Accounts => _accounts.Value;
        public IUserOtpRepository UserOtps => _userOtps.Value;
        public IUserDocumentRepository UserDocuments => _userDocuments.Value;
        public IUserAddressRepository UserAddresses => _userAddresses.Value;
        public IUserContributionSchemeRepository UserContributionSchemes => _userContributionSchemes.Value;
        public IUserKYCRepository UserKYC => _userKYC.Value;
        public IUserWithdrawalSettingRepository UserWithdrawalSettings => _userWithdrawalSettings.Value;
        public IUserContributionRepository UserContributions => _userContributions.Value;
        public IRecentActivityRepository RecentActivities => _recentActivities.Value;
        #endregion

        #region Contributions
        public IContributionSchemeRepository ContributionSchemes => _contributionSchemes.Value;
        #endregion

        #region Notifications
        public INotificationRepository Notifications => _notifications.Value;
        #endregion

        #region Finances
        public IBankRepository Banks => _banks.Value;
        public IWalletRepository Wallets => _wallets.Value;
        public ITransactionRepository Transactions => _transactions.Value;
        public IPaymentRepository Payments => _payments.Value;
        public ILinkedCardRepository LinkedCards => _linkedCards.Value;
        #endregion

        #region Utility
        public IContactUsMailRepository ContactUsMails => _contactUsMails.Value;
        #endregion

        #region Loans
        public ILoanApplicationRepository LoanApplications => _loanApplications.Value;
        public IApprovedLoanRepository ApprovedLoans => _approvedLoans.Value;
        public ILoanRepaymentRepository LoanRepayments => _loanRepayments.Value;
        #endregion

        #region Admin Portal
        public IUserManagementRepository UserManagement => _userManagement.Value;
        public IMessageTemplateRepository MessageTemplates => _messageTemplates.Value;
        public IDashboardRepository Dashboard => _dashboard.Value;
        public ICommunicationRepository Communications => _communications.Value;
        public ICommunicationRecipientRepository CommunicationRecipients => _communicationRecipients.Value;
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

        public ValueTask DisposeAsync()
        {
            if (_sqlDbContext != null)
            {
                return _sqlDbContext.DisposeAsync();
            }

            if (_transaction != null)
            {
                return _transaction.DisposeAsync();
            }

            return ValueTask.CompletedTask;
        }

        private static Lazy<T> NewLazy<T>(IServiceProvider sp) where T : notnull =>
            new(sp.GetRequiredService<T>, LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion
    }
}

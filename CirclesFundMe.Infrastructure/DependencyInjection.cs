namespace CirclesFundMe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<SqlDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            #region Users
            services.AddScoped<ICFMAccountRepository, CFMAccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserOtpRepository, UserOtpRepository>();
            services.AddScoped<IUserDocumentRepository, UserDocumentRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IUserKYCRepository, UserKYCRepository>();
            services.AddScoped<IUserContributionSchemeRepository, UserContributionSchemeRepository>();
            services.AddScoped<IUserWithdrawalSettingRepository, UserWithdrawalSettingRepository>();
            services.AddScoped<IUserContributionRepository, UserContributionRepository>();
            services.AddScoped<IRecentActivityRepository, RecentActivityRepository>();
            #endregion

            #region Contributions
            services.AddScoped<IContributionSchemeRepository, ContributionSchemeRepository>();
            #endregion

            #region Notifications
            services.AddScoped<INotificationRepository, NotificationRepository>();
            #endregion

            #region Finances
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ILinkedCardRepository, LinkedCardRepository>();
            #endregion

            #region Utility
            services.AddScoped<IContactUsMailRepository, ContactUsMailRepository>();
            #endregion

            #region Loans
            services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
            services.AddScoped<IApprovedLoanRepository, ApprovedLoanRepository>();
            services.AddScoped<ILoanRepaymentRepository, LoanRepaymentRepository>();
            #endregion

            #region Admin Portal
            services.AddScoped<IUserManagementRepository, UserManagementRepository>();
            services.AddScoped<IMessageTemplateRepository, MessageTemplateRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ICommunicationRepository, CommunicationRepository>();
            services.AddScoped<ICommunicationRecipientRepository, CommunicationRecipientRepository>();
            #endregion

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}

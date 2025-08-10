using CirclesFundMe.Domain.Entities.AdminPortal;

namespace CirclesFundMe.Infrastructure.Data.DbContexts
{
    public class SqlDbContext(DbContextOptions<SqlDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        #region Account & Users
        public DbSet<CFMAccount> CFMAccounts { get; set; }
        public DbSet<UserOtp> UserOtps { get; set; }
        public DbSet<UserContributionScheme> UserContributionSchemes { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserDocument> UserDocuments { get; set; }
        public DbSet<UserKYC> UserKYCs { get; set; }
        public DbSet<UserWithdrawalSetting> UserWithdrawalSettings { get; set; }
        public DbSet<UserContribution> UserContributions { get; set; }
        public DbSet<RecentActivity> RecentActivities { get; set; }
        #endregion

        #region Contributions
        public DbSet<ContributionScheme> ContributionSchemes { get; set; }
        #endregion

        #region Notifications
        public DbSet<Notification> Notifications { get; set; }
        #endregion

        #region Finances
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<LinkedCard> LinkedCards { get; set; }
        #endregion

        #region Utility
        public DbSet<ContactUsMail> ContactUsMails { get; set; }
        #endregion

        #region Loans
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<ApprovedLoan> ApprovedLoans { get; set; }
        public DbSet<LoanRepayment> LoanRepayments { get; set; }
        #endregion

        #region Admin Portal
        public DbSet<MessageTemplate> MessageTemplates { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("CFM");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationConfig).Assembly);
        }
    }
}

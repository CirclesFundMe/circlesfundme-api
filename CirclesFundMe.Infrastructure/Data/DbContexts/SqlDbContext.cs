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
        #endregion

        #region Contributions
        public DbSet<ContributionScheme> ContributionSchemes { get; set; }
        #endregion

        #region Notifications
        public DbSet<Notification> Notifications { get; set; }
        #endregion

        #region Finances
        public DbSet<Bank> Banks { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("CFM");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationConfig).Assembly);
        }
    }
}

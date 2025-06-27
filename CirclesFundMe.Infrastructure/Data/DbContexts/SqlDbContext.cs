namespace CirclesFundMe.Infrastructure.Data.DbContexts
{
    public class SqlDbContext(DbContextOptions<SqlDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        #region Account & Users
        public DbSet<CFMAccount> CFMAccounts { get; set; }
        public DbSet<UserOtp> UserOtps { get; set; }
        #endregion

        #region Notifications
        public DbSet<Notification> Notifications { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("CFM");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationConfig).Assembly);
        }
    }
}

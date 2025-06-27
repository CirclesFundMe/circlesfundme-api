namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class CFMAccountRepository(DbSet<CFMAccount> accounts) : RepositoryBase<CFMAccount>(accounts), ICFMAccountRepository
    {
        private readonly DbSet<CFMAccount> _accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));

        public async Task<PagedList<CFMAccount>> GetAccounts(CFMAccountParams accountParams, CancellationToken cancellation)
        {
            IQueryable<CFMAccount> query = _accounts.AsNoTracking().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(accountParams.SearchKey))
            {
                query = query.Where(x => x.Name.Contains(accountParams.SearchKey));
            }

            return await PagedList<CFMAccount>.ToPagedListAsync(query.OrderBy(x => x.Name), accountParams.PageNumber, accountParams.PageSize, cancellation);
        }
    }
}

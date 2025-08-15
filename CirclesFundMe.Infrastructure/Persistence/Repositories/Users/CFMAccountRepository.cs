namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class CFMAccountRepository(SqlDbContext context) : RepositoryBase<CFMAccount>(context.CFMAccounts), ICFMAccountRepository
    {
        private readonly DbSet<CFMAccount> _accounts = context.CFMAccounts;

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

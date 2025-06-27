namespace CirclesFundMe.Domain.RepositoryContracts.Users
{
    public interface ICFMAccountRepository : IRepositoryBase<CFMAccount>
    {
        Task<PagedList<CFMAccount>> GetAccounts(CFMAccountParams accountParams, CancellationToken cancellation);
    }
}

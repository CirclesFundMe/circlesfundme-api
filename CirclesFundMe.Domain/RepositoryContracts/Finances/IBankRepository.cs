namespace CirclesFundMe.Domain.RepositoryContracts.Finances
{
    public interface IBankRepository : IRepositoryBase<Bank>
    {
        Task<IEnumerable<Bank>> GetBanks(CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetBankCodes(CancellationToken cancellationToken = default);
    }
}

namespace CirclesFundMe.Domain.RepositoryContracts.Finances
{
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        Task<PagedList<Transaction>> GetTransactionsByWalletId(Guid walletId, TransactionParams transactionParams, CancellationToken cancellation);
    }
}

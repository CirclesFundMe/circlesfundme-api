namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Finances
{
    public class TransactionRepository(DbSet<Transaction> transactions) : RepositoryBase<Transaction>(transactions), ITransactionRepository
    {
        private readonly DbSet<Transaction> _transactions = transactions;

        public async Task<PagedList<Transaction>> GetTransactionsByWalletId(Guid walletId, TransactionParams transactionParams, CancellationToken cancellation)
        {
            IQueryable<Transaction> query = _transactions
                .Where(t => t.WalletId == walletId)
                .AsNoTracking();

            if (Enum.IsDefined(transactionParams.TransactionType))
            {
                query = query.Where(t => t.TransactionType == transactionParams.TransactionType);
            }

            int count = await query.CountAsync(cancellation);

            var items = await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip((transactionParams.PageNumber - 1) * transactionParams.PageSize)
                .Take(transactionParams.PageSize)
                .Select(t => new Transaction
                {
                    Id = t.Id,
                    TransactionReference = t.TransactionReference,
                    Narration = t.Narration,
                    TransactionType = t.TransactionType,
                    BalanceBeforeTransaction = t.BalanceBeforeTransaction,
                    Amount = t.Amount,
                    BalanceAfterTransaction = t.BalanceAfterTransaction,
                    TransactionDate = t.TransactionDate,
                    TransactionTime = t.TransactionTime,
                    SessionId = t.SessionId,
                    WalletId = t.WalletId
                })
                .ToListAsync(cancellation);

            return new PagedList<Transaction>(items, count, transactionParams.PageNumber, transactionParams.PageSize);
        }
    }
}

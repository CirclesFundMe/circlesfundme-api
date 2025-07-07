namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Finances
{
    public class TransactionRepository(DbSet<Transaction> transactions) : RepositoryBase<Transaction>(transactions), ITransactionRepository
    {
    }
}


namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Finances
{
    public class BankRepository(SqlDbContext context) : RepositoryBase<Bank>(context.Banks), IBankRepository
    {
        private readonly DbSet<Bank> _banks = context.Banks;

        public async Task<IEnumerable<string>> GetBankCodes(CancellationToken cancellationToken = default)
        {
            return await _banks
                .AsNoTracking()
                .Select(b => b.Code)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Bank>> GetBanks(CancellationToken cancellationToken = default)
        {
            return await _banks
                .AsNoTracking()
                .OrderBy(b => b.Name)
                .Select(b => new Bank
                {
                    Code = b.Code,
                    Name = b.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}

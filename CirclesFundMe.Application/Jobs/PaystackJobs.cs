namespace CirclesFundMe.Application.Jobs
{
    public class PaystackJobs(IServiceScopeFactory serviceScopeFactory, ILogger<PaystackJobs> logger, UtilityHelper utility)
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<PaystackJobs> _logger = logger;
        private readonly UtilityHelper _utility = utility;

        public async Task SynchronizeBanks()
        {
            try
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                IPaystackClient paystackClient = scope.ServiceProvider.GetRequiredService<IPaystackClient>();
                SqlDbContext dbContext = scope.ServiceProvider.GetRequiredService<SqlDbContext>();

                await _utility.ExecuteWithRetryAsync(async () =>
                {
                    string? next = null;
                    do
                    {
                        BankDataQuery query = new() { PerPage = 100, UseCursor = true, Next = next };
                        var response = await paystackClient.GetBanksList(query);

                        if (response?.data == null)
                            break;

                        // Go through the retrieved banks and check if the code currently exists in the database
                        IEnumerable<string> existingCodes = await dbContext.Banks
                                                                            .AsNoTracking()
                                                                            .Select(b => b.Code)
                                                                            .ToListAsync(CancellationToken.None);

                        // Insert new banks into the database if they do not exist
                        List<Bank> newBanks = response.data
                            .Where(b => !existingCodes.Contains(b.code) && !string.IsNullOrEmpty(b.code) && !string.IsNullOrEmpty(b.name))
                            .Select(b => new Bank
                            {
                                Code = b.code!,
                                Name = b.name!
                            })
                            .DistinctBy(x => x.Code)
                            .ToList();

                        if (newBanks.Count != 0)
                        {
                            await dbContext.Banks.AddRangeAsync(newBanks, CancellationToken.None);
                            await dbContext.SaveChangesAsync(CancellationToken.None);
                            dbContext.ChangeTracker.Clear();
                        }

                        // Use the Next cursor to fetch the next set of banks if available
                        next = response.meta?.next;
                    } while (!string.IsNullOrEmpty(next));
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while synchronizing banks from Paystack.");
            }
        }
    }
}

namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Loans
{
    public class LoanApplicationRepository(DbSet<LoanApplication> loans) : RepositoryBase<LoanApplication>(loans), ILoanApplicationRepository
    {
        private readonly DbSet<LoanApplication> _loans = loans;

        public async Task<PagedList<LoanApplication>> GetLoanApplications(LoanApplicationParams @params, CancellationToken cancellationToken)
        {
            IQueryable<LoanApplication> query = _loans.AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (Enum.IsDefined(@params.Status))
            {
                query = query.Where(x => x.Status == @params.Status);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(@params.PageSize * (@params.PageNumber - 1))
                .Take(@params.PageSize)
                .Select(x => new LoanApplication
                {
                    Id = x.Id,
                    Status = x.Status,
                    ApprovedAmount = x.ApprovedAmount,
                    User = x.User != null ? new AppUser
                    {
                        Id = x.User.Id,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Email = x.User.Email
                    } : null,
                })
                .ToListAsync(cancellationToken);

            return new PagedList<LoanApplication>(items, totalCount, @params.PageNumber, @params.PageSize);
        }
    }
}

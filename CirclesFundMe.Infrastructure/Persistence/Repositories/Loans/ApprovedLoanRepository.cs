
namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Loans
{
    public class ApprovedLoanRepository(SqlDbContext context) : RepositoryBase<ApprovedLoan>(context.ApprovedLoans), IApprovedLoanRepository
    {
        private readonly DbSet<ApprovedLoan> _approvedLoans = context.ApprovedLoans;

        public async Task<PagedList<ApprovedLoanExtension>> GetApprovedLoanHistoryByUserId(string userId, ApprovedLoanParams @params, CancellationToken cancellationToken = default)
        {
            IQueryable<ApprovedLoan> query = _approvedLoans.AsNoTracking()
                .Where(loan => loan.UserId == userId);

            if (Enum.IsDefined(@params.Status))
            {
                query = query.Where(loan => loan.Status == @params.Status);
            }

            int count = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(loan => loan.ApprovedDate)
                .Skip(@params.PageSize * (@params.PageNumber - 1))
                .Take(@params.PageSize)
                .Select(loan => new ApprovedLoanExtension
                {
                    Id = loan.Id,
                    Status = loan.Status,
                    ApprovedAmount = loan.ApprovedAmount,
                    ApprovedDate = loan.ApprovedDate,
                    LoanApplicationId = loan.LoanApplicationId,
                    UserId = loan.UserId,
                    AmountRepaid = loan.LoanRepayments.Where(r => r.Status == LoanRepaymentStatusEnums.Paid).Sum(repayment => repayment.Amount),
                    FirstRepaymentDate = loan.LoanRepayments.Min(repayment => repayment.RepaymentDate) ?? DateTime.MinValue,
                    LastRepaymentDate = loan.LoanRepayments.Max(repayment => repayment.RepaymentDate) ?? DateTime.MinValue,
                    RepaymentCount = loan.LoanRepayments.Count(r => r.Status == LoanRepaymentStatusEnums.Paid),
                    TotalRepaymentCount = loan.LoanRepayments.Count(),
                })
                .ToListAsync(cancellationToken);

            return new PagedList<ApprovedLoanExtension>(items, count, @params.PageNumber, @params.PageSize);
        }
    }
}

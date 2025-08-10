namespace CirclesFundMe.Infrastructure.Persistence.Repositories.AdminPortal
{
    public class DashboardRepository(SqlDbContext context) : IDashboardRepository
    {
        private readonly SqlDbContext _context = context;

        public async Task<DashboardStatistics> GetDashboardStatisticsAsync(CancellationToken cancellationToken)
        {
            int totalPendingKYCs = await _context.Users
                .CountAsync(u => u.UserKYC == null
                                            || string.IsNullOrEmpty(u.UserKYC.BVN)
                                            || !u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.Selfie)
                                            || !u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.UtilityBill)
                                            || !u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.GovernmentIssuedId), cancellationToken);

            int totalActiveLoans = await _context.ApprovedLoans
                .CountAsync(l => l.Status == ApprovedLoanStatusEnums.Active, cancellationToken);

            int totalOverduePayments = await _context.LoanRepayments
                .CountAsync(p => p.Status == LoanRepaymentStatusEnums.Overdue, cancellationToken);

            int totalUsers = await _context.Users.CountAsync(cancellationToken);

            return new DashboardStatistics
            {
                TotalPendingKYCs = totalPendingKYCs,
                TotalActiveLoans = totalActiveLoans,
                TotalOverduePayments = totalOverduePayments,
                TotalUsers = totalUsers
            };
        }
    }
}

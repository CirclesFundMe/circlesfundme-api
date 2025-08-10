namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Loans
{
    public class ApprovedLoanRepository(SqlDbContext context) : RepositoryBase<ApprovedLoan>(context.ApprovedLoans), IApprovedLoanRepository
    {
    }
}

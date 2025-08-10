namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Loans
{
    public class LoanRepaymentRepository(SqlDbContext context) : RepositoryBase<LoanRepayment>(context.LoanRepayments), ILoanRepaymentRepository
    {
    }
}

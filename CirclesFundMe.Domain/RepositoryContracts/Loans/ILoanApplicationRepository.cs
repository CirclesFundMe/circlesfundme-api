namespace CirclesFundMe.Domain.RepositoryContracts.Loans
{
    public interface ILoanApplicationRepository : IRepositoryBase<LoanApplication>
    {
        Task<PagedList<LoanApplication>> GetLoanApplications(LoanApplicationParams @params, CancellationToken cancellationToken);
    }
}

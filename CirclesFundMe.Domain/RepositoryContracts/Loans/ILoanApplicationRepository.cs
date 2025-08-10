namespace CirclesFundMe.Domain.RepositoryContracts.Loans
{
    public interface ILoanApplicationRepository : IRepositoryBase<LoanApplication>
    {
        Task<PagedList<LoanApplicationExtension>> GetLoanApplications(LoanApplicationParams @params, CancellationToken cancellationToken);
        Task<LoanApplicationExtension?> GetLoanApplicationById(Guid loanApplicationId, CancellationToken cancellationToken);
    }
}

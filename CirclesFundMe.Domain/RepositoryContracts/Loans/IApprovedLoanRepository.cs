namespace CirclesFundMe.Domain.RepositoryContracts.Loans
{
    public interface IApprovedLoanRepository : IRepositoryBase<ApprovedLoan>
    {
        Task<PagedList<ApprovedLoanExtension>> GetApprovedLoanHistoryByUserId(string userId, ApprovedLoanParams @params, CancellationToken cancellationToken = default);
    }
}

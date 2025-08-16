namespace CirclesFundMe.Application.CQRS.QueryHandlers.Loans
{
    public class GetMyLoanHistoryQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<GetMyLoanHistoryQuery, BaseResponse<PagedList<LoanHistoryModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<PagedList<LoanHistoryModel>>> Handle(GetMyLoanHistoryQuery request, CancellationToken cancellationToken)
        {
            PagedList<ApprovedLoanExtension> approvedLoans = await _unitOfWork.ApprovedLoans.GetApprovedLoanHistoryByUserId(_currentUserService.UserId, request.Params, cancellationToken);

            List<LoanHistoryModel> loanHistoryModels = approvedLoans.Select(loan => new LoanHistoryModel
            {
                AmountRepaid = loan.AmountRepaid,
                FirstRepaymentDate = loan.FirstRepaymentDate,
                LastRepaymentDate = loan.LastRepaymentDate,
                Status = loan.Status.ToString(),
                RepaymentCount = loan.RepaymentCount,
                TotalRepaymentCount = loan.TotalRepaymentCount
            }).ToList();

            PagedList<LoanHistoryModel> pagedLoanHistoryModels = new(loanHistoryModels, approvedLoans.TotalCount, approvedLoans.CurrentPage, approvedLoans.PageSize);

            return new BaseResponse<PagedList<LoanHistoryModel>>
            {
                Data = pagedLoanHistoryModels,
                MetaData = PagedListHelper<LoanHistoryModel>.GetPaginationInfo(pagedLoanHistoryModels)
            };
        }
    }
}

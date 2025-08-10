namespace CirclesFundMe.Application.CQRS.QueryHandlers.AdminPortal
{
    public class GetAdminInflowQueryHandler(IUnitOfWork unitOfWork, IOptions<AppSettings> options) : IRequestHandler<GetAdminInflowQuery, BaseResponse<PagedList<AdminTransactionModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AppSettings _appSettings = options.Value;
        public async Task<BaseResponse<PagedList<AdminTransactionModel>>> Handle(GetAdminInflowQuery request, CancellationToken cancellationToken)
        {
            TransactionParams @params = new()
            {
                PageNumber = request.Params.PageNumber,
                PageSize = request.Params.PageSize,
                SearchKey = request.Params.SearchKey,
                TransactionType = TransactionTypeEnums.Credit,
            };

            PagedList<Transaction> transactions = await _unitOfWork.Transactions.GetTransactionsByWalletId(_appSettings.GLWalletId, @params, cancellationToken);

            List<AdminTransactionModel> adminTransactions = transactions.Select(t => new AdminTransactionModel
            {
                Narration = t.Narration,
                Amount = t.Amount,
            }).ToList();

            return new()
            {
                Message = "Admin inflow transactions retrieved successfully.",
                Data = new PagedList<AdminTransactionModel>(adminTransactions, transactions.TotalCount, transactions.CurrentPage, transactions.PageSize),
                MetaData = PagedListHelper<Transaction>.GetPaginationInfo(transactions)
            };
        }
    }
}

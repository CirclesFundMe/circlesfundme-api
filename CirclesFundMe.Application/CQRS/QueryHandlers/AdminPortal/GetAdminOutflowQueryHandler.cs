
namespace CirclesFundMe.Application.CQRS.QueryHandlers.AdminPortal
{
    public class GetAdminOutflowQueryHandler(IUnitOfWork unitOfWork, IOptions<AppSettings> options) : IRequestHandler<GetAdminOutflowQuery, BaseResponse<PagedList<AdminTransactionModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AppSettings _appSettings = options.Value;

        public async Task<BaseResponse<PagedList<AdminTransactionModel>>> Handle(GetAdminOutflowQuery request, CancellationToken cancellationToken)
        {
            TransactionParams @params = new()
            {
                PageNumber = request.Params.PageNumber,
                PageSize = request.Params.PageSize,
                SearchKey = request.Params.SearchKey,
                TransactionType = TransactionTypeEnums.Debit,
            };

            PagedList<Transaction> transactions = await _unitOfWork.Transactions.GetTransactionsByWalletId(_appSettings.GLWalletId, @params, cancellationToken);

            List<AdminTransactionModel> adminTransactions = transactions.Select(t => new AdminTransactionModel
            {
                Narration = t.Narration,
                Amount = t.Amount,
            }).ToList();

            return new()
            {
                Message = "Admin outflow transactions retrieved successfully.",
                Data = new PagedList<AdminTransactionModel>(adminTransactions, transactions.TotalCount, transactions.CurrentPage, transactions.PageSize),
                MetaData = PagedListHelper<Transaction>.GetPaginationInfo(transactions)
            };
        }
    }
}

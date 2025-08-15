namespace CirclesFundMe.Application.CQRS.QueryHandlers.AdminPortal
{
    public class GetUserPaymentsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserPaymentsQuery, BaseResponse<PagedList<PaymentAdminModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<PagedList<PaymentAdminModel>>> Handle(GetUserPaymentsQuery request, CancellationToken cancellationToken)
        {
            PagedList<PaymentAdmin> userPayments = await _unitOfWork.Payments.GetUserPaymentsForAdmin(request.UserId!, request.Params, cancellationToken);

            List<PaymentAdminModel> res = userPayments
                .Select(x => new PaymentAdminModel
                {
                    Date = x.Date,
                    Action = x.Action,
                    Amount = x.Amount,
                    Charge = x.Charge,
                    Status = x.Status.ToString()
                }).ToList();

            return new()
            {
                Message = "User payments retrieved successfully.",
                Data = PagedList<PaymentAdminModel>.ToPagedList(res, userPayments.TotalCount, userPayments.CurrentPage, userPayments.PageSize),
                MetaData = PagedListHelper<PaymentAdmin>.GetPaginationInfo(userPayments)
            };
        }
    }
}

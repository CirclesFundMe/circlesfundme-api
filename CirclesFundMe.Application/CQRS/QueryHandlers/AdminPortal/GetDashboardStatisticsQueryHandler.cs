namespace CirclesFundMe.Application.CQRS.QueryHandlers.AdminPortal
{
    public class GetDashboardStatisticsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDashboardStatisticsQuery, BaseResponse<DashboardStatisticsModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<DashboardStatisticsModel>> Handle(GetDashboardStatisticsQuery request, CancellationToken cancellationToken)
        {
            DashboardStatistics statistics = await _unitOfWork.Dashboard.GetDashboardStatisticsAsync(cancellationToken);

            return new()
            {
                Data = new()
                {
                    TotalPendingKYCs = statistics.TotalPendingKYCs,
                    TotalActiveLoans = statistics.TotalActiveLoans,
                    TotalOverduePayments = statistics.TotalOverduePayments,
                    TotalUsers = statistics.TotalUsers
                },
                Message = "Dashboard statistics retrieved successfully."
            };
        }
    }
}

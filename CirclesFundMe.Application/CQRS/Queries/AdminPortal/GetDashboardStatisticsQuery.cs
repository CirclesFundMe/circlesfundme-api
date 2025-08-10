namespace CirclesFundMe.Application.CQRS.Queries.AdminPortal
{
    public record GetDashboardStatisticsQuery : IRequest<BaseResponse<DashboardStatisticsModel>>
    {
    }
}

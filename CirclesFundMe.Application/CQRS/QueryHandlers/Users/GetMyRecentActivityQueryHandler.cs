namespace CirclesFundMe.Application.CQRS.QueryHandlers.Users
{
    public class GetMyRecentActivityQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) :
        IRequestHandler<GetMyRecentActivityQuery, BaseResponse<PagedList<RecentActivityModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly string _userId = currentUserService.UserId;

        public async Task<BaseResponse<PagedList<RecentActivityModel>>> Handle(GetMyRecentActivityQuery request, CancellationToken cancellationToken)
        {
            PagedList<RecentActivity> recentActivities = await _unitOfWork.RecentActivities.GetMyRecentActivities(_userId, request.Params, cancellationToken);

            List<RecentActivityModel> res = recentActivities
                .Select(x => new RecentActivityModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type,
                    Data = x.Data
                }).ToList();

            return new()
            {
                Message = "Recent activities retrieved successfully.",
                Data = PagedList<RecentActivityModel>.ToPagedList(res, recentActivities.TotalCount, recentActivities.CurrentPage, recentActivities.PageSize),
            };
        }
    }
}

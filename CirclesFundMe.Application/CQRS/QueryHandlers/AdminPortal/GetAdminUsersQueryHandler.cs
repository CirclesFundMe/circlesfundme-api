namespace CirclesFundMe.Application.CQRS.QueryHandlers.AdminPortal
{
    public class GetAdminUsersQueryHandler(IUnitOfWork unitOfWork, UtilityHelper utility) : IRequestHandler<GetAdminUsersQuery, BaseResponse<PagedList<AdminUserModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UtilityHelper _utility = utility;

        public async Task<BaseResponse<PagedList<AdminUserModel>>> Handle(GetAdminUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserManagement.GetUsersAsync(request.Params, cancellationToken);

            var userModels = users.Select(user => new AdminUserModel
            {
                UserId = user.UserId,
                Name = user.Name,
                DateJoined = user.DateJoined,
                Scheme = user.Scheme,
                TotalContribution = user.TotalContribution,
                EligibleLoan = ExtractEligibleLoan(user.SchemeType, user.CopyOfCurrentBreakdownAtOnboarding!),
                TotalRepaidAmount = user.TotalRepaidAmount,
                IsActive = !user.IsDeleted
            }).ToList();

            return new BaseResponse<PagedList<AdminUserModel>>
            {
                Data = new PagedList<AdminUserModel>(userModels, users.TotalCount, request.Params.PageNumber, request.Params.PageSize),
                MetaData = PagedListHelper<AppUserAdmin>.GetPaginationInfo(users),
                Message = "Users retrieved successfully."
            };
        }

        private decimal ExtractEligibleLoan(SchemeTypeEnums schemeType, string CopyOfCurrentBreakdownAtOnboarding)
        {
            decimal eligibleLoan = 0;

            if (schemeType == SchemeTypeEnums.AutoFinance)
            {
                var autoFinanceBreakdown = _utility.Deserializer<AutoFinanceBreakdownModel>(CopyOfCurrentBreakdownAtOnboarding);
                if (autoFinanceBreakdown != null)
                {
                    eligibleLoan = Convert.ToDecimal(autoFinanceBreakdown.EligibleLoan);
                }
            }
            else
            {
                var regularLoanBreakdown = _utility.Deserializer<RegularLoanBreakdownModel>(CopyOfCurrentBreakdownAtOnboarding);
                if (regularLoanBreakdown != null)
                {
                    eligibleLoan = Convert.ToDecimal(regularLoanBreakdown.EligibleLoan);
                }
            }

            return eligibleLoan;
        }
    }
}


namespace CirclesFundMe.Application.CQRS.QueryHandlers.Users
{
    public class ViewMyEligibleLoanQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, UtilityHelper utility) : IRequestHandler<ViewMyEligibleLoanQuery, BaseResponse<object>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public readonly string _currentUserId = currentUserService.UserId;
        private readonly UtilityHelper _utility = utility;

        public async Task<BaseResponse<object>> Handle(ViewMyEligibleLoanQuery request, CancellationToken cancellationToken)
        {
            UserContributionScheme? userContributionScheme = await _unitOfWork.UserContributionSchemes.ViewMyEligibleLoan(_currentUserId, cancellationToken);

            if (userContributionScheme == null)
            {
                return BaseResponse<object>.NotFound("You are not eligible for a loan at this time.");
            }

            object? res = userContributionScheme.ContributionScheme!.SchemeType != SchemeTypeEnums.AutoFinance
                ? _utility.Deserializer<RegularLoanBreakdownModel>(userContributionScheme.CopyOfCurrentBreakdownAtOnboarding!)
                : _utility.Deserializer<AutoFinanceBreakdownModel>(userContributionScheme.CopyOfCurrentBreakdownAtOnboarding!);

            if (res == null)
            {
                return BaseResponse<object>.BadRequest("Failed to retrieve your eligible loan details. Please try again later.");
            }

            return BaseResponse<object>.Success(res, "Your eligible loan details retrieved successfully.");
        }
    }
}

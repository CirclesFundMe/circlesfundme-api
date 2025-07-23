
namespace CirclesFundMe.Application.CQRS.QueryHandlers.Contributions
{
    public class GetAutoFinanceBreakdownQueryHandler(IUnitOfWork unitOfWork) :
        IRequestHandler<GetAutoFinanceBreakdownQuery, BaseResponse<AutoFinanceBreakdownModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<AutoFinanceBreakdownModel>> Handle(GetAutoFinanceBreakdownQuery request, CancellationToken cancellationToken)
        {
            if (request.CostOfVehicle <= 0)
            {
                return BaseResponse<AutoFinanceBreakdownModel>.BadRequest("Cost of vehicle must be greater than zero.");
            }

            (AutoFinanceBreakdown? breakdown, string? message) = await _unitOfWork.ContributionSchemes.GetAutoFinanceBreakdown(request.CostOfVehicle, cancellationToken);
            if (breakdown == null)
            {
                return BaseResponse<AutoFinanceBreakdownModel>.BadRequest(message!);
            }

            Dictionary<string, string> formatted = UtilityHelper.FormatDecimalProperties(breakdown);

            AutoFinanceBreakdownModel breakdownModel = new()
            {
                BaseContributionAmount = breakdown.BaseFee,
                CostOfVehicle = formatted["CostOfVehicle"],
                ExtraEngine = formatted["ExtraEngine"],
                ExtraTyre = formatted["ExtraTyre"],
                Insurance = formatted["Insurance"],
                ProcessingFee = formatted["ProcessingFee"],
                TotalAssetValue = formatted["TotalAssetValue"],
                DownPayment = formatted["DownPayment"],
                LoanManagementFee = formatted["LoanManagementFee"],
                PreLoanServiceCharge = formatted["PreLoanServiceCharge"],
                PostLoanWeeklyContribution = formatted["PostLoanWeeklyContribution"],
                EligibleLoan = formatted["EligibleLoan"],
                TotalRepayment = formatted["TotalRepayment"]
            };

            return BaseResponse<AutoFinanceBreakdownModel>.Success(breakdownModel, "Auto finance breakdown calculated successfully.");
        }
    }
}

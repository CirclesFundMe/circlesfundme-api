namespace CirclesFundMe.Application.CQRS.QueryHandlers.Contributions
{
    public class GetEligibleLoanDetailQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetEligibleLoanDetailQuery, BaseResponse<RegularLoanBreakdownModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<BaseResponse<RegularLoanBreakdownModel>> Handle(GetEligibleLoanDetailQuery request, CancellationToken cancellationToken)
        {
            RegularFinanceBreakdown? financeBreakdown = await _unitOfWork.ContributionSchemes.GetRegularFinanceBreakdown(request.ContributionSchemeId, request.Amount, cancellationToken);

            if (financeBreakdown == null)
            {
                return BaseResponse<RegularLoanBreakdownModel>.NotFound("Unable to get finance breakdown. Invalid parameter(s)");
            }

            string weekOrMonth = financeBreakdown.SchemeType == SchemeTypeEnums.Weekly ? "week" : "month";

            RegularLoanBreakdownModel eligibleLoanDetail = new()
            {
                PrincipalLoan = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.PrincipalLoan),
                PrincipalLoanDescription = FormServiceValueDescription(financeBreakdown),
                LoanManagementFee = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.LoanManagementFee),
                LoanManagementFeeDescription = "Loan Mgt. Fee",
                EligibleLoan = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.EligibleLoan),
                EligibleLoanDescription = "Principal Loan - Loan Mgt. Fee",
                ServiceCharge = $"{UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.ServiceCharge)}/{weekOrMonth}",
                TotalRepayment = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.TotalRepayment),
                RepaymentTerm = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.RepaymentTerm),
            };

            return BaseResponse<RegularLoanBreakdownModel>.Success(eligibleLoanDetail, "Eligible loan details retrieved successfully.");
        }

        private static string FormServiceValueDescription(RegularFinanceBreakdown breakdown)
        {
            if (breakdown.SchemeType == SchemeTypeEnums.Weekly || breakdown.SchemeType == SchemeTypeEnums.Monthly)
            {
                string word = breakdown.SchemeType == SchemeTypeEnums.Weekly ? "weekly" : "monthly";
                return $"{breakdown.LoanMultiple}x of your {word} contribution";
            }

            return string.Empty;
        }
    }
}

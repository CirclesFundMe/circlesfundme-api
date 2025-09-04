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

            string weekMonthOrDaily = FrequencyText(financeBreakdown.SchemeType);

            RegularLoanBreakdownModel eligibleLoanDetail = new()
            {
                PrincipalLoan = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.PrincipalLoan),
                PrincipalLoanDescription = FormServiceValueDescription(financeBreakdown),
                LoanManagementFee = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.LoanManagementFee),
                LoanManagementFeeDescription = "Loan Mgt. Fee",
                EligibleLoan = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.EligibleLoan),
                EligibleLoanDescription = "Principal Loan - Loan Mgt. Fee",
                PreLoanServiceCharge = $"{UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.PreLoanServiceCharge)}/{weekMonthOrDaily}",
                PostLoanServiceCharge = $"{UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.PostLoanServiceCharge)}/{weekMonthOrDaily}",
                TotalRepayment = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.TotalRepayment),
                RepaymentTerm = UtilityHelper.FormatDecimalToNairaWithSymbol(financeBreakdown.RepaymentTerm),
            };

            return BaseResponse<RegularLoanBreakdownModel>.Success(eligibleLoanDetail, "Eligible loan details retrieved successfully.");
        }

        private string FrequencyText(SchemeTypeEnums schemeType)
        {
            return schemeType switch
            {
                SchemeTypeEnums.Weekly => "week",
                SchemeTypeEnums.Monthly => "month",
                SchemeTypeEnums.Daily => "day",
                _ => "period"
            };
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

namespace CirclesFundMe.Application.CQRS.QueryHandlers.Contributions
{
    public class GetEligibleLoanDetailQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetEligibleLoanDetailQuery, BaseResponse<EligibleLoanDetailModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<BaseResponse<EligibleLoanDetailModel>> Handle(GetEligibleLoanDetailQuery request, CancellationToken cancellationToken)
        {
            ContributionScheme? scheme = await _unitOfWork.ContributionSchemes.GetByPrimaryKey(request.ContributionSchemeId, cancellationToken);

            if (scheme == null)
            {
                return BaseResponse<EligibleLoanDetailModel>.NotFound("Contribution scheme not found. Please check the ID and try again.");
            }

            if (scheme.SchemeType == SchemeTypeEnums.AutoFinance)
            {
                return BaseResponse<EligibleLoanDetailModel>.NotFound("Eligible loan details are not available for Auto Finance schemes.");
            }

            EligibleLoanDetailModel eligibleLoanDetail = new()
            {
                ContributionSchemeName = scheme.SchemeType.ToString(),
                EligibleLoanMultiple = scheme.EligibleLoanMultiple,
                EligibleLoanDescription = FormEligibilityDescriptionByScheme(scheme),
                ServiceChargeAmount = (decimal)scheme.ServiceCharge,
                ServiceChargeDescription = FormServiceChargeDescriptionByScheme(scheme)
            };

            return BaseResponse<EligibleLoanDetailModel>.Success(eligibleLoanDetail, "Eligible loan details retrieved successfully.");
        }

        private static string FormServiceChargeDescriptionByScheme(ContributionScheme scheme)
        {
            if (scheme.SchemeType == SchemeTypeEnums.Weekly || scheme.SchemeType == SchemeTypeEnums.Monthly)
            {
                return scheme.SchemeType == SchemeTypeEnums.Weekly ? "/week" : "/month";
            }

            return string.Empty;
        }

        private static string FormEligibilityDescriptionByScheme(ContributionScheme scheme)
        {
            if (scheme.SchemeType == SchemeTypeEnums.Weekly || scheme.SchemeType == SchemeTypeEnums.Monthly)
            {
                string word = scheme.SchemeType == SchemeTypeEnums.Weekly ? "weekly contribution" : "monthly contribution";

                return $"{scheme.EligibleLoanMultiple}x of your {word}";
            }

            return string.Empty;
        }
    }
}

namespace CirclesFundMe.Application.CQRS.QueryHandlers.Loans
{
    public class GetLoanApplicationByIdQueryHandler(IUnitOfWork unitOfWork, UtilityHelper utility) : IRequestHandler<GetLoanApplicationByIdQuery, BaseResponse<LoanApplicationDetailModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UtilityHelper _utility = utility;

        public async Task<BaseResponse<LoanApplicationDetailModel>> Handle(GetLoanApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            LoanApplicationExtension? loan = await _unitOfWork.LoanApplications.GetLoanApplicationById(request.LoanApplicationId, cancellationToken);

            if (loan == null)
            {
                return BaseResponse<LoanApplicationDetailModel>.NotFound("Unable to find this loan detail");
            }

            decimal loanManagementFee;
            decimal repaymentTerm;
            if (loan.Scheme == SchemeTypeEnums.AutoFinance)
            {
                AutoFinanceBreakdown? autoBreakdown = _utility.Deserializer<AutoFinanceBreakdown>(loan.Breakdown!);

                if (autoBreakdown == null)
                {
                    return BaseResponse<LoanApplicationDetailModel>.NotFound("Unable to find this loan detail breakdown");
                }

                loanManagementFee = autoBreakdown.LoanManagementFee;
                repaymentTerm = autoBreakdown.PostLoanWeeklyContribution;
            }
            else
            {
                RegularFinanceBreakdown? regularBreakdown = _utility.Deserializer<RegularFinanceBreakdown>(loan.Breakdown!);

                if (regularBreakdown == null)
                {
                    return BaseResponse<LoanApplicationDetailModel>.NotFound("Unable to find this loan detail breakdown");
                }

                loanManagementFee = regularBreakdown.LoanManagementFee;
                repaymentTerm = regularBreakdown.RepaymentTerm;
            }

            LoanApplicationDetailModel loanApplicationDetailModel = new()
            {
                Id = loan.Id,
                Fullname = $"{loan.User?.FirstName} {loan.User?.LastName}",
                AccountNumber = loan.User?.WithdrawalSetting?.AccountNumber,
                Bank = loan.User?.WithdrawalSetting?.Bank?.Name,
                Scheme = loan.Scheme.ToString(),
                IncomeAmount = loan.User?.UserContributionScheme?.IncomeAmount ?? 0,
                ContributionAmount = loan.User?.UserContributionScheme?.ContributionAmount ?? 0,
                TotalContribution = loan.TotalContribution,
                BVN = loan.User?.UserKYC?.BVN,
                RequestedAmount = loan.RequestedAmount,
                LoanManagementFee = loanManagementFee,
                RepaymentTerm = repaymentTerm,
                IsEligible = true, // Ideally, nobody can apply for loan if not eligible
                Status = loan.Status.ToString()
            };

            return BaseResponse<LoanApplicationDetailModel>.Success(loanApplicationDetailModel, "Loan application detail retrieved successfully");
        }
    }
}

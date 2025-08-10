namespace CirclesFundMe.Application.CQRS.CommandHandlers.Loans
{
    public class CreateLoanApplicationCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, UtilityHelper utility) : IRequestHandler<CreateLoanApplicationCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly string _currentUserId = currentUserService.UserId;
        private readonly UtilityHelper _utility = utility;

        public async Task<BaseResponse<bool>> Handle(CreateLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            LoanApplication? loanApplication = await _unitOfWork.LoanApplications.GetOneAsync([x => x.UserId == _currentUserId,
                x => x.Status != LoanApplicationStatusEnums.Approved], cancellationToken);

            if (loanApplication != null)
            {
                return BaseResponse<bool>.BadRequest("You already have an unapproved loan application.");
            }

            UserContributionScheme? userContributionScheme = await _unitOfWork.UserContributionSchemes.ViewMyEligibleLoan(_currentUserId, cancellationToken);

            if (userContributionScheme == null)
            {
                return BaseResponse<bool>.NotFound("You are not eligible for a loan at this time.");
            }

            decimal eligibleAmount = 0;
            if (userContributionScheme.ContributionScheme!.SchemeType != SchemeTypeEnums.AutoFinance)
            {
                RegularLoanBreakdownModel? breakdown = _utility.Deserializer<RegularLoanBreakdownModel>(userContributionScheme.CopyOfCurrentBreakdownAtOnboarding!);
                if (breakdown == null)
                {
                    return BaseResponse<bool>.BadRequest("Failed to retrieve your eligible loan details. Please try again later.");
                }
                eligibleAmount = Convert.ToDecimal(breakdown.EligibleLoan);
            }
            else
            {
                AutoFinanceBreakdownModel? breakdown = _utility.Deserializer<AutoFinanceBreakdownModel>(userContributionScheme.CopyOfCurrentBreakdownAtOnboarding!);
                if (breakdown == null)
                {
                    return BaseResponse<bool>.BadRequest("Failed to retrieve your eligible loan details. Please try again later.");
                }
                eligibleAmount = Convert.ToDecimal(breakdown.EligibleLoan);
            }

            if (request.RequestedLoanAmount.HasValue && (request.RequestedLoanAmount > eligibleAmount))
            {
                return BaseResponse<bool>.BadRequest($"You can only apply for a loan of up to {eligibleAmount:N2}.");
            }

            loanApplication = new LoanApplication
            {
                UserId = _currentUserId,
                Status = LoanApplicationStatusEnums.Pending,
                RequestedAmount = request.RequestedLoanAmount ?? eligibleAmount,
                ApprovedAmount = 0
            };

            await _unitOfWork.LoanApplications.AddAsync(loanApplication, cancellationToken);
            bool isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (isSaved)
            {
                return BaseResponse<bool>.Success(true, "Loan application created successfully.");
            }
            else
            {
                return BaseResponse<bool>.BadRequest("Failed to create loan application. Our engineers are investigating this issue");
            }
        }
    }
}

namespace CirclesFundMe.Application.CQRS.CommandHandlers.Loans
{
    public class ApproveLoanApplicationCommandHandler(IUnitOfWork unitOfWork, UtilityHelper utility) : IRequestHandler<ApproveLoanApplicationCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UtilityHelper _utility = utility;
        public async Task<BaseResponse<bool>> Handle(ApproveLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            // Get the loan application by ID
            LoanApplication? loanApplication = await _unitOfWork.LoanApplications.GetByPrimaryKey(request.LoanApplicationId, cancellationToken);

            if (loanApplication == null)
            {
                return BaseResponse<bool>.NotFound("Loan application not found.");
            }

            if (loanApplication.Status != LoanApplicationStatusEnums.Pending && loanApplication.Status != LoanApplicationStatusEnums.Waitlist)
            {
                return BaseResponse<bool>.BadRequest("This loan application is not in a state that can be approved.");
            }

            // Check if the user has an active contribution scheme
            UserContributionScheme? userContributionScheme = await _unitOfWork.UserContributionSchemes.GetOneAsync([x => x.UserId == loanApplication.UserId], cancellationToken);

            if (userContributionScheme == null)
            {
                return BaseResponse<bool>.NotFound("User scheme not found.");
            }

            // Retrieve the user's loan wallet
            Wallet? userWallet = await _unitOfWork.Wallets.GetOneAsync([x => x.UserId == loanApplication.UserId && x.Type == WalletTypeEnums.Loan], cancellationToken);
            if (userWallet == null)
            {
                return BaseResponse<bool>.NotFound("User loan wallet not found.");
            }

            decimal eligibleAmount;
            decimal repaymentTerm;
            int numberOfInstallments;
            decimal totalRepaymentAmount;
            DateTime repaymentDate = DateTime.UtcNow;

            if (loanApplication.Scheme != SchemeTypeEnums.AutoFinance)
            {
                RegularLoanBreakdownModel? breakdown = _utility.Deserializer<RegularLoanBreakdownModel>(userContributionScheme.CopyOfCurrentBreakdownAtOnboarding!);
                if (breakdown == null)
                {
                    return BaseResponse<bool>.BadRequest("Failed to retrieve eligible loan details. Please try again later.");
                }
                eligibleAmount = Convert.ToDecimal(breakdown.EligibleLoan);
                repaymentTerm = Convert.ToDecimal(breakdown.RepaymentTerm);
                totalRepaymentAmount = Convert.ToDecimal(breakdown.TotalRepayment);

                if (userContributionScheme.IsWeeklyRoutine)
                {
                    numberOfInstallments = 52;
                    repaymentDate = UtilityHelper.GetNextWeekDay(repaymentDate, userContributionScheme.ContributionWeekDay);
                }
                else
                {
                    numberOfInstallments = 12;
                    repaymentDate = UtilityHelper.GetNextMonthDay(repaymentDate, userContributionScheme.ContributionMonthDay);
                }
            }
            else
            {
                AutoFinanceBreakdownModel? breakdown = _utility.Deserializer<AutoFinanceBreakdownModel>(userContributionScheme.CopyOfCurrentBreakdownAtOnboarding!);
                if (breakdown == null)
                {
                    return BaseResponse<bool>.BadRequest("Failed to retrieve eligible loan details. Please try again later.");
                }
                eligibleAmount = Convert.ToDecimal(breakdown.EligibleLoan);
                repaymentTerm = Convert.ToDecimal(breakdown.PostLoanWeeklyContribution);
                totalRepaymentAmount = Convert.ToDecimal(breakdown.TotalRepayment);
                numberOfInstallments = 208;

                if (userContributionScheme.IsWeeklyRoutine)
                {
                    repaymentDate = UtilityHelper.GetNextWeekDay(repaymentDate, userContributionScheme.ContributionWeekDay);
                }
                else
                {
                    repaymentDate = UtilityHelper.GetNextWeekDay(repaymentDate, WeekDayEnums.Saturday);
                }
            }

            // Create the approved loan record
            ApprovedLoan approvedLoan = new()
            {
                Id = Guid.NewGuid(),
                Status = ApprovedLoanStatusEnums.Active,
                ApprovedAmount = eligibleAmount,
                ApprovedDate = DateTime.UtcNow,
                LoanApplicationId = loanApplication.Id,
                UserId = loanApplication.UserId
            };
            await _unitOfWork.ApprovedLoans.AddAsync(approvedLoan, cancellationToken);

            // Generate the loan repayments
            List<LoanRepayment> repayments = [];

            DateTime dueDate = repaymentDate;
            for (int i = 0; i < numberOfInstallments; i++)
            {
                LoanRepayment repayment = new()
                {
                    Amount = repaymentTerm,
                    DueDate = dueDate,
                    Status = LoanRepaymentStatusEnums.Unpaid,
                    ApprovedLoanId = approvedLoan.Id
                };
                repayments.Add(repayment);

                if (loanApplication.Scheme != SchemeTypeEnums.AutoFinance)
                {
                    if (userContributionScheme.IsWeeklyRoutine)
                    {
                        dueDate = UtilityHelper.GetNextWeekDay(dueDate, userContributionScheme.ContributionWeekDay);
                    }
                    else
                    {
                        dueDate = UtilityHelper.GetNextMonthDay(dueDate, userContributionScheme.ContributionMonthDay);
                    }
                }
                else
                {
                    if (userContributionScheme.IsWeeklyRoutine)
                    {
                        dueDate = UtilityHelper.GetNextWeekDay(dueDate, userContributionScheme.ContributionWeekDay);
                    }
                    else
                    {
                        dueDate = UtilityHelper.GetNextWeekDay(dueDate, WeekDayEnums.Saturday);
                    }
                }
            }
            await _unitOfWork.LoanRepayments.AddRangeAsync(repayments, cancellationToken);

            // Update loan application status
            loanApplication.Status = LoanApplicationStatusEnums.Approved;
            loanApplication.CurrentEligibleAmount = eligibleAmount;
            _unitOfWork.LoanApplications.Update(loanApplication);

            // Update user wallet balance
            userWallet.Balance -= totalRepaymentAmount;
            userWallet.NextTranDate = repaymentDate;

            bool isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            return BaseResponse<bool>.Success(isSaved, "Loan application approved successfully.");
        }
    }
}

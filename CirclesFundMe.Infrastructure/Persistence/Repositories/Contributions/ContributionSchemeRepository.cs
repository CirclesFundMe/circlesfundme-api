namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Contributions
{
    public class ContributionSchemeRepository(DbSet<ContributionScheme> contributionSchemes)
        : RepositoryBase<ContributionScheme>(contributionSchemes), IContributionSchemeRepository
    {
        private readonly DbSet<ContributionScheme> _contributionSchemes = contributionSchemes;

        public async Task<(AutoFinanceBreakdown? breakdown, string? message)> GetAutoFinanceBreakdown(decimal costOfVehicle, CancellationToken cancellation)
        {
            ContributionScheme? scheme = await _contributionSchemes
                .AsNoTracking()
                .Where(cs => cs.SchemeType == SchemeTypeEnums.AutoFinance)
                .FirstOrDefaultAsync(cancellationToken: cancellation);

            if (scheme == null)
            {
                return (null, "Contribution scheme not found");
            }

            if (costOfVehicle < (decimal)scheme.MinimumVehicleCost)
            {
                return (null, $"Cost of Vehicle must be at least {Math.Round(scheme.MinimumVehicleCost).ToString("N0", CultureInfo.InvariantCulture)}");
            }

            decimal extraEngine = costOfVehicle * (decimal)scheme.ExtraEnginePercent / 100;
            decimal extraTyre = costOfVehicle * (decimal)scheme.ExtraTyrePercent / 100;
            decimal insurance = (costOfVehicle * (decimal)scheme.InsurancePerAnnumPercent / 100) * 4;
            decimal processingFee = (costOfVehicle + extraEngine + extraTyre + insurance) * (decimal)scheme.ProcessingFeePercent / 100;
            decimal totalAssetValue = costOfVehicle + extraEngine + extraTyre + insurance + processingFee;
            decimal preLoanServiceCharge = (totalAssetValue * (decimal)scheme.PreLoanServiceChargePercent) / 100;
            decimal downPayment = totalAssetValue * (decimal)scheme.DownPaymentPercent / 100;

            decimal actualLoanAmount = totalAssetValue - downPayment;
            decimal loanManagementFee = actualLoanAmount * (decimal)scheme.LoanManagementFeePercent / 100 * 4;

            decimal postLoanServiceCharge = (actualLoanAmount + loanManagementFee) * (decimal)scheme.PostLoanServiceChargePercent / 100;
            decimal totalRepaymentAmount = (postLoanServiceCharge * 48) + actualLoanAmount + loanManagementFee;

            decimal postLoanWeeklyContribution = totalRepaymentAmount / 208;

            return (new AutoFinanceBreakdown
            {
                CostOfVehicle = costOfVehicle,
                ExtraEngine = extraEngine,
                ExtraTyre = extraTyre,
                Insurance = insurance,
                ProcessingFee = processingFee,
                TotalAssetValue = totalAssetValue,
                DownPayment = downPayment,
                LoanManagementFee = loanManagementFee,
                PreLoanServiceCharge = preLoanServiceCharge,
                PostLoanWeeklyContribution = postLoanWeeklyContribution,
                BaseFee = (decimal)scheme.BaseFee,
                TotalRepayment = totalRepaymentAmount
            }, "Here is your auto breakdown");
        }

        public async Task<List<ContributionScheme>> GetContributionSchemes(CancellationToken cancellationToken)
        {
            return await _contributionSchemes
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ContributionScheme>> GetContributionSchemesMini(CancellationToken cancellationTokens)
        {
            return await _contributionSchemes
                .AsNoTracking()
                .Select(cs => new ContributionScheme
                {
                    Id = cs.Id,
                    Name = cs.Name,
                    SchemeType = cs.SchemeType
                })
                .ToListAsync(cancellationTokens);
        }

        public async Task<RegularFinanceBreakdown?> GetRegularFinanceBreakdown(Guid schemeId, decimal amount, CancellationToken cancellation)
        {
            ContributionScheme? scheme = await _contributionSchemes
                .AsNoTracking()
                .Where(cs => cs.Id == schemeId)
                .FirstOrDefaultAsync(cancellationToken: cancellation);

            if (scheme == null)
            {
                return null;
            }

            if (scheme.SchemeType != SchemeTypeEnums.Weekly && scheme.SchemeType != SchemeTypeEnums.Monthly)
            {
                return null;
            }

            decimal principalLoan = amount * (decimal)scheme.EligibleLoanMultiple;
            decimal serviceCharge = principalLoan * (decimal)scheme.ServiceCharge / 100;
            decimal totalServiceCharge = scheme.SchemeType == SchemeTypeEnums.Weekly
                ? serviceCharge * 52
                : serviceCharge * 12;
            decimal downPayment = principalLoan * (decimal)scheme.DownPaymentPercent / 100;
            decimal loanManagementFee = principalLoan * (decimal)scheme.LoanManagementFeePercent / 100;
            decimal totalRepayment = principalLoan + totalServiceCharge;

            decimal repaymentTerm = scheme.SchemeType == SchemeTypeEnums.Weekly
                ? totalRepayment / 52
                : totalRepayment / 12;

            int countToQualifyForLoan = scheme.SchemeType == SchemeTypeEnums.Weekly
                ? 12 // 12 weeks
                : 3; // 3 months

            return new RegularFinanceBreakdown
            {
                PrincipalLoan = principalLoan,
                LoanManagementFee = loanManagementFee,
                ServiceCharge = serviceCharge,
                SchemeType = scheme.SchemeType,
                LoanMultiple = (int)scheme.EligibleLoanMultiple,
                TotalRepayment = totalRepayment,
                RepaymentTerm = repaymentTerm,
                DownPayment = downPayment,
                CountToQualifyForLoan = countToQualifyForLoan,
            };
        }
    }
}

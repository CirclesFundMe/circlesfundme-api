namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Contributions
{
    public class ContributionSchemeRepository(SqlDbContext context)
        : RepositoryBase<ContributionScheme>(context.ContributionSchemes), IContributionSchemeRepository
    {
        private readonly DbSet<ContributionScheme> _contributionSchemes = context.ContributionSchemes;

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

            if (scheme.SchemeType != SchemeTypeEnums.Weekly && scheme.SchemeType != SchemeTypeEnums.Monthly && scheme.SchemeType != SchemeTypeEnums.Daily)
            {
                return null;
            }

            decimal principalLoan = amount * (decimal)scheme.EligibleLoanMultiple;

            decimal preLoanServiceCharge = principalLoan * (decimal)scheme.PreLoanServiceChargePercent / 100;

            decimal postLoanServiceCharge = principalLoan * (decimal)scheme.PostLoanServiceChargePercent / 100;

            decimal downPayment = principalLoan * (decimal)scheme.DownPaymentPercent / 100;

            decimal loanManagementFee = principalLoan * (decimal)scheme.LoanManagementFeePercent / 100;

            decimal totalRepayment = principalLoan + (postLoanServiceCharge * (decimal)scheme.EligibleLoanMultiple);

            decimal repaymentTerm = totalRepayment / GetLoanTerm(scheme.SchemeType);

            int countToQualifyForLoan = GetLoanQualificationTerm(scheme.SchemeType);

            return new RegularFinanceBreakdown
            {
                PrincipalLoan = principalLoan,
                LoanManagementFee = loanManagementFee,
                PreLoanServiceCharge = preLoanServiceCharge,
                PostLoanServiceCharge = postLoanServiceCharge,
                SchemeType = scheme.SchemeType,
                LoanMultiple = (int)scheme.EligibleLoanMultiple,
                TotalRepayment = totalRepayment,
                RepaymentTerm = repaymentTerm,
                DownPayment = downPayment,
                CountToQualifyForLoan = countToQualifyForLoan,
            };
        }

        private static int GetLoanQualificationTerm(SchemeTypeEnums schemeType)
        {
            return schemeType switch
            {
                SchemeTypeEnums.Weekly => 12, // 12 weeks to qualify
                SchemeTypeEnums.Monthly => 3, // 3 months to qualify
                SchemeTypeEnums.Daily => 90, // 90 days to qualify
                _ => 1,
            };
        }

        private static int GetLoanTerm(SchemeTypeEnums schemeType)
        {
            return schemeType switch
            {
                SchemeTypeEnums.Weekly => 52, // 52 weeks repayment
                SchemeTypeEnums.Monthly => 12, // 12 months repayment
                SchemeTypeEnums.Daily => 365, // 365 days repayment
                _ => 1,
            };
        }

        private static decimal GetCumulativeCharge(decimal unitCharge, SchemeTypeEnums schemeType, bool isPreLoan = false)
        {
            if (isPreLoan)
            {
                return schemeType switch
                {
                    SchemeTypeEnums.Weekly => unitCharge * 12, // 12 weeks to qualify
                    SchemeTypeEnums.Monthly => unitCharge * 3, // 3 months to qualify
                    SchemeTypeEnums.Daily => unitCharge * 90, // 90 days to qualify
                    _ => unitCharge,
                };
            }
            else
            {
                return schemeType switch
                {
                    SchemeTypeEnums.Weekly => unitCharge * 52, // 40 weeks repayment
                    SchemeTypeEnums.Monthly => unitCharge * 12, // 12 months repayment
                    SchemeTypeEnums.Daily => unitCharge * 365, // 365 days repayment
                    _ => unitCharge,
                };
            }
        }
    }
}

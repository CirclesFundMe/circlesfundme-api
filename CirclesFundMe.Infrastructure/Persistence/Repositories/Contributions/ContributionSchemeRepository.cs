namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Contributions
{
    public class ContributionSchemeRepository(DbSet<ContributionScheme> contributionSchemes)
        : RepositoryBase<ContributionScheme>(contributionSchemes), IContributionSchemeRepository
    {
        private readonly DbSet<ContributionScheme> _contributionSchemes = contributionSchemes;

        public async Task<AutoFinanceBreakdown?> GetAutoFinanceBreakdown(decimal costOfVehicle, CancellationToken cancellation)
        {
            ContributionScheme? scheme = await _contributionSchemes
                .AsNoTracking()
                .Where(cs => cs.SchemeType == SchemeTypeEnums.AutoFinance)
                .FirstOrDefaultAsync(cancellationToken: cancellation);

            if (scheme == null)
            {
                return null;
            }

            decimal extraEngine = (costOfVehicle * (decimal)scheme.ExtraEnginePercent) / 100;
            decimal extraTyre = (costOfVehicle * (decimal)scheme.ExtraTyrePercent) / 100;
            decimal insurance = ((costOfVehicle * (decimal)scheme.InsurancePerAnnumPercent) / 100) * 4;
            decimal processingFee = (costOfVehicle * (decimal)scheme.ProcessingFeePercent) / 100;
            decimal totalAssetValue = costOfVehicle + extraEngine + extraTyre + insurance + processingFee;
            decimal downPayment = (totalAssetValue * (decimal)scheme.DownPaymentPercent) / 100;

            decimal actualLoanAmount = totalAssetValue - downPayment;
            decimal loanManagementFee = actualLoanAmount * (decimal)scheme.LoanManagementFeePercent / 100 * 4;

            decimal preLoanServiceCharge = (totalAssetValue * (decimal)scheme.PreLoanServiceChargePercent) / 100;

            decimal postLoanServiceCharge = (actualLoanAmount + loanManagementFee) * (decimal)scheme.PostLoanServiceChargePercent / 100;
            decimal totalRepaymentAmount = (postLoanServiceCharge * 48) + actualLoanAmount + loanManagementFee;

            decimal minimumWeeklyContribution = (totalAssetValue / 208) + preLoanServiceCharge;
            decimal postLoanWeeklyContribution = (totalRepaymentAmount / 208);

            return new AutoFinanceBreakdown
            {
                CostOfVehicle = costOfVehicle,
                ExtraEngine = extraEngine,
                ExtraTyre = extraTyre,
                Insurance = insurance,
                ProcessingFee = processingFee,
                TotalAssetValue = totalAssetValue,
                DownPayment = downPayment,
                LoanManagementFee = loanManagementFee,
                MinimumWeeklyContribution = minimumWeeklyContribution,
                PostLoanWeeklyContribution = postLoanWeeklyContribution
            };
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
    }
}

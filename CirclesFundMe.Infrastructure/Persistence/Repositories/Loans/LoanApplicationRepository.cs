namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Loans
{
    public class LoanApplicationRepository(SqlDbContext context) : RepositoryBase<LoanApplication>(context.LoanApplications), ILoanApplicationRepository
    {
        private readonly DbSet<LoanApplication> _loans = context.LoanApplications;

        public async Task<LoanApplicationExtension?> GetLoanApplicationById(Guid loanApplicationId, CancellationToken cancellationToken)
        {
            return await _loans.AsNoTracking()
                .Where(x => x.Id == loanApplicationId && !x.IsDeleted)
                .Select(x => new LoanApplicationExtension
                {
                    Id = x.Id,
                    Status = x.Status,
                    RequestedAmount = x.RequestedAmount,
                    CurrentEligibleAmount = x.CurrentEligibleAmount,
                    CreatedDate = x.CreatedDate,
                    Scheme = x.Scheme,
                    Breakdown = x.Breakdown,
                    User = x.User != null ? new AppUser
                    {
                        Id = x.User.Id,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Email = x.User.Email,
                        UserKYC = x.User.UserKYC != null ? new UserKYC
                        {
                            Id = x.User.UserKYC.Id,
                            BVN = x.User.UserKYC.BVN,
                        } : null,
                        UserContributionScheme = x.User.UserContributionScheme != null ? new UserContributionScheme
                        {
                            Id = x.User.UserContributionScheme.Id,
                            IncomeAmount = x.User.UserContributionScheme.IncomeAmount,
                            ContributionAmount = x.User.UserContributionScheme.ContributionAmount,
                            ContributionScheme = x.User.UserContributionScheme.ContributionScheme != null ? new ContributionScheme
                            {
                                Id = x.User.UserContributionScheme.ContributionScheme.Id,
                                Name = x.User.UserContributionScheme.ContributionScheme.Name,
                                SchemeType = x.User.UserContributionScheme.ContributionScheme.SchemeType
                            } : null
                        } : null,
                        WithdrawalSetting = x.User.WithdrawalSetting != null ? new UserWithdrawalSetting
                        {
                            Id = x.User.WithdrawalSetting.Id,
                            AccountNumber = x.User.WithdrawalSetting.AccountNumber,
                            AccountName = x.User.WithdrawalSetting.AccountName,
                            Bank = x.User.WithdrawalSetting.Bank != null ? new Bank
                            {
                                Id = x.User.WithdrawalSetting.Bank.Id,
                                Name = x.User.WithdrawalSetting.Bank.Name,
                                Code = x.User.WithdrawalSetting.Bank.Code
                            } : null
                        } : null
                    } : null,
                    AmountRepaid = x.ApprovedLoan != null ? x.ApprovedLoan.LoanRepayments.Sum(x => x.Amount) : 0,
                    TotalContribution = x.User!.UserContributions != null ? x.User.UserContributions.Sum(c => c.Amount) : 0
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PagedList<LoanApplicationExtension>> GetLoanApplications(LoanApplicationParams @params, CancellationToken cancellationToken)
        {
            IQueryable<LoanApplication> query = _loans.AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (Enum.IsDefined(@params.Status))
            {
                query = query.Where(x => x.Status == @params.Status);
            }

            if (Enum.IsDefined(@params.SchemeType))
            {
                query = query.Where(x => x.User != null
                && x.User.UserContributionScheme != null
                && x.User.UserContributionScheme.ContributionScheme != null
                && x.User.UserContributionScheme.ContributionScheme.SchemeType == @params.SchemeType);
            }

            if (!string.IsNullOrEmpty(@params.UserId))
            {
                query = query.Where(x => x.UserId == @params.UserId);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(@params.PageSize * (@params.PageNumber - 1))
                .Take(@params.PageSize)
                .Select(x => new LoanApplicationExtension
                {
                    Id = x.Id,
                    Status = x.Status,
                    RequestedAmount = x.RequestedAmount,
                    CurrentEligibleAmount = x.CurrentEligibleAmount,
                    ApprovedAmount = x.ApprovedLoan != null ? x.ApprovedLoan.ApprovedAmount : 0,
                    CreatedDate = x.CreatedDate,
                    User = x.User != null ? new AppUser
                    {
                        Id = x.User.Id,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Email = x.User.Email,
                        ProfilePictureUrl = x.User.ProfilePictureUrl,
                        UserContributionScheme = x.User.UserContributionScheme != null ? new UserContributionScheme
                        {
                            Id = x.User.UserContributionScheme.Id,
                            ContributionScheme = x.User.UserContributionScheme.ContributionScheme != null ? new ContributionScheme
                            {
                                Id = x.User.UserContributionScheme.ContributionScheme.Id,
                                Name = x.User.UserContributionScheme.ContributionScheme.Name,
                                SchemeType = x.User.UserContributionScheme.ContributionScheme.SchemeType
                            } : null
                        } : null,
                    } : null,
                    AmountRepaid = x.ApprovedLoan != null ? x.ApprovedLoan.LoanRepayments.Sum(x => x.Amount) : 0
                })
                .ToListAsync(cancellationToken);

            return new PagedList<LoanApplicationExtension>(items, totalCount, @params.PageNumber, @params.PageSize);
        }
    }
}

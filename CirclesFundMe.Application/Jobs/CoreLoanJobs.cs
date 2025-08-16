namespace CirclesFundMe.Application.Jobs
{
    public class CoreLoanJobs(EmailService emailService, IServiceScopeFactory serviceScopeFactory, ILogger<CFMJobs> logger, UtilityHelper utility, IOptions<AppSettings> options)
    {
        private readonly EmailService _emailService = emailService;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<CFMJobs> _logger = logger;
        private readonly UtilityHelper _utility = utility;
        private readonly AppSettings _appSettings = options.Value;

        public async Task GenerateUserContributionSchedule(string userId, Guid userContributionSchemeId)
        {
            await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();
            IServiceProvider sp = scope.ServiceProvider;
            SqlDbContext dbContext = scope.ServiceProvider.GetRequiredService<SqlDbContext>();
            IUserManagementRepository userManagementRepository = scope.ServiceProvider.GetRequiredService<IUserManagementRepository>();
            IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            try
            {
                await _utility.ExecuteWithRetryAsync(async () =>
                {
                    UserContributionScheme? userContributionScheme = await unitOfWork.UserContributionSchemes.GetByPrimaryKeys([userId, userContributionSchemeId], CancellationToken.None);

                    if (userContributionScheme == null)
                    {
                        _logger.LogWarning($"User Contribution Scheme with ID {userContributionSchemeId} not found.");
                        return;
                    }

                    DateTime dueDate = userContributionScheme.CommencementDate;
                    int numberOfInstallments = userContributionScheme.CountToQualifyForLoan;

                    List<UserContribution> contributions = [];

                    for (int i = 0; i < numberOfInstallments; i++)
                    {
                        UserContribution contribution = new()
                        {
                            Amount = userContributionScheme.ActualContributionAmount,
                            AmountIncludingCharges = userContributionScheme.ContributionAmount,
                            Charges = userContributionScheme.ChargeAmount,
                            Status = UserContributionStatusEnums.Unpaid,
                            DueDate = dueDate,
                            UserId = userContributionScheme.UserId
                        };

                        contributions.Add(contribution);

                        if (userContributionScheme.IsWeeklyRoutine)
                        {
                            dueDate = UtilityHelper.GetNextWeekDay(dueDate, userContributionScheme.ContributionWeekDay);
                        }
                        else
                        {
                            dueDate = UtilityHelper.GetNextMonthDay(dueDate, userContributionScheme.ContributionMonthDay);
                        }
                    }

                    await dbContext.UserContributions.AddRangeAsync(contributions, CancellationToken.None);
                    await unitOfWork.SaveChangesAsync(CancellationToken.None);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while generating contribution schedule for User Contribution Scheme {userContributionSchemeId}.");
            }
        }
    }
}

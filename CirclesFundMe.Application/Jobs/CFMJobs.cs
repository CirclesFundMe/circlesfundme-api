﻿namespace CirclesFundMe.Application.Jobs
{
    public class CFMJobs(EmailService emailService, IServiceScopeFactory serviceScopeFactory, ILogger<CFMJobs> logger, UtilityHelper utility)
    {
        private readonly EmailService _emailService = emailService;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<CFMJobs> _logger = logger;
        private readonly UtilityHelper _utility = utility;

        public async Task SendOTP(string emailAddress, string otp, string? firstName)
        {
            try
            {
                await _utility.ExecuteWithRetryAsync(async () =>
                {
                    string userName = firstName ?? "Member";

                    StringBuilder sb = new(_emailService.LoadHtmlTemplate("otp"));
                    sb.Replace("{{FirstName}}", userName);
                    sb.Replace("{{OTP}}", otp);
                    sb.Replace("{{Year}}", DateTime.UtcNow.Year.ToString());

                    EmailMessage msg = new(emailAddress, "Security Code", sb.ToString(), null);

                    await _emailService.SendEmail(msg);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while sending OTP to {emailAddress}.");
            }
        }

        public async Task CreateWalletsForNewUser(string userId)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            SqlDbContext dbContext = scope.ServiceProvider.GetRequiredService<SqlDbContext>();

            try
            {
                await _utility.ExecuteWithRetryAsync(async () =>
                {
                    List<Wallet> existingWallets = await dbContext.Wallets
                    .Where(w => w.UserId == userId)
                    .ToListAsync(CancellationToken.None);

                    if (existingWallets.Count > 0)
                    {
                        _logger.LogInformation($"Wallets already exist for user {userId}. No new wallets created.");
                        return;
                    }

                    List<Wallet> newWallets =
                    [
                        new Wallet
                    {
                        Balance = 0,
                        Type = WalletTypeEnums.Contribution,
                        Status = WalletStatusEnums.Active,
                        UserId = userId
                    },
                    new Wallet
                    {
                        Balance = 0,
                        Type = WalletTypeEnums.Loan,
                        Status = WalletStatusEnums.Active,
                        UserId = userId
                    }
                    ];

                    await dbContext.Wallets.AddRangeAsync(newWallets, CancellationToken.None);
                    await dbContext.SaveChangesAsync(CancellationToken.None);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating wallet for user {userId}.");
            }
        }

        public async Task SendNotification(IEnumerable<CreateNotificationModel> models)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            SqlDbContext dbContext = scope.ServiceProvider.GetRequiredService<SqlDbContext>();

            try
            {
                await _utility.ExecuteWithRetryAsync(async () =>
                {
                    if (models == null || !models.Any())
                    {
                        _logger.LogWarning("No notification models provided to send.");
                        return;
                    }

                    List<Notification> notifications = [];
                    foreach (CreateNotificationModel model in models)
                    {
                        if (string.IsNullOrWhiteSpace(model.UserId))
                        {
                            _logger.LogWarning("Notification model does not have a UserId. Skipping notification creation.");
                            continue;
                        }

                        Notification notification = new()
                        {
                            Title = model.Title,
                            Metadata = model.Metadata,
                            Type = model.Type,
                            ObjectId = model.ObjectId,
                            UserId = model.UserId,
                            IsRead = false,
                            ShouldSendEmailNotification = true,
                            EmailNotificationSubject = "New Notification"
                        };
                        notifications.Add(notification);
                    }

                    if (notifications.Count == 0)
                    {
                        _logger.LogWarning("No valid notifications to create.");
                        return;
                    }

                    await dbContext.Notifications.AddRangeAsync(notifications, CancellationToken.None);
                    await dbContext.SaveChangesAsync(CancellationToken.None);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while sending notifications");
            }
        }
    }
}

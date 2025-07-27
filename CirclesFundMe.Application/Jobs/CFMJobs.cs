namespace CirclesFundMe.Application.Jobs
{
    public class CFMJobs(EmailService emailService, IServiceScopeFactory serviceScopeFactory, ILogger<CFMJobs> logger, UtilityHelper utility, IOptions<AppSettings> options)
    {
        private readonly EmailService _emailService = emailService;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<CFMJobs> _logger = logger;
        private readonly UtilityHelper _utility = utility;
        private readonly AppSettings _appSettings = options.Value;

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

        public async Task SendContactUsMail(string? firstName, string? lastName, string? email, string? phone, string? title, string? message)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            SqlDbContext dbContext = scope.ServiceProvider.GetRequiredService<SqlDbContext>();

            try
            {
                await _utility.ExecuteWithRetryAsync(async () =>
                {
                    ContactUsMail contactUsMail = new()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        Phone = phone,
                        Title = title,
                        Message = message,
                        IsMailSentToAdmin = false
                    };

                    await dbContext.ContactUsMails.AddAsync(contactUsMail, CancellationToken.None);
                    await dbContext.SaveChangesAsync(CancellationToken.None);

                    _logger.LogInformation("Contact Us mail saved successfully.");

                    foreach (AdminContact adminContact in _appSettings.AdminContacts)
                    {
                        StringBuilder sb = new(_emailService.LoadHtmlTemplate("contactus"));
                        sb.Replace("{{FirstName}}", firstName);
                        sb.Replace("{{LastName}}", lastName);
                        sb.Replace("{{Email}}", email);
                        sb.Replace("{{Phone}}", phone);
                        sb.Replace("{{Title}}", title);
                        sb.Replace("{{Message}}", message);
                        sb.Replace("{{Year}}", DateTime.UtcNow.Year.ToString());
                        sb.Replace("{{AdminName}}", adminContact.Name);

                        EmailMessage msg = new(adminContact.Email, "New Contact Us Message", sb.ToString(), null);
                        await _emailService.SendEmail(msg);
                    }

                    contactUsMail.IsMailSentToAdmin = true;
                    contactUsMail.ModifiedDate = DateTime.UtcNow;

                    dbContext.ContactUsMails.Update(contactUsMail);
                    await dbContext.SaveChangesAsync(CancellationToken.None);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending Contact Us mail.");
            }
        }

        public async Task CreateRecentActivity(RecentActivity recentActivity)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            SqlDbContext dbContext = scope.ServiceProvider.GetRequiredService<SqlDbContext>();

            try
            {
                await _utility.ExecuteWithRetryAsync(async () =>
                {
                    if (recentActivity == null)
                    {
                        _logger.LogWarning("RecentActivity is null. Skipping creation.");
                        return;
                    }

                    await dbContext.RecentActivities.AddAsync(recentActivity, CancellationToken.None);
                    await dbContext.SaveChangesAsync(CancellationToken.None);

                    _logger.LogInformation("Recent activity created successfully for user {UserId}", recentActivity.UserId);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating recent activity for user {UserId}", recentActivity.UserId);
            }
        }
    }
}

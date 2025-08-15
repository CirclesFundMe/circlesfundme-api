namespace CirclesFundMe.Application.Jobs
{
    public class CommunicationJobs(IServiceScopeFactory serviceScopeFactory, ILogger<PaystackJobs> logger, UtilityHelper utility)
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<PaystackJobs> _logger = logger;
        private readonly UtilityHelper _utility = utility;

        public async Task ProcessCommunicationQueue()
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            SqlDbContext _context = scope.ServiceProvider.GetRequiredService<SqlDbContext>();
            EmailService _emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
            IUserManagementRepository _userManagementRepository = scope.ServiceProvider.GetRequiredService<IUserManagementRepository>();
            IUnitOfWork _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            try
            {
                await _utility.ExecuteWithRetryAsync(async () =>
                {
                    // Fetch all pending communication tasks
                    List<Communication> queuedComms = await _context.Communications
                        .Where(t => t.Status == CommunicationStatus.Queued || (t.Status == CommunicationStatus.Failed && t.RetryCount < 4))
                        .ToListAsync(CancellationToken.None);

                    if (queuedComms.Count == 0)
                    {
                        return;
                    }

                    foreach (var comm in queuedComms)
                    {
                        try
                        {
                            comm.Status = CommunicationStatus.Processing;
                            _context.Communications.Update(comm);
                            await _context.SaveChangesAsync(CancellationToken.None);

                            IEnumerable<AppUser> users = await _unitOfWork.UserManagement.GetUsersByCommunicationTarget(comm.Target);

                            if (users == null || !users.Any())
                            {
                                comm.Status = CommunicationStatus.Failed;
                                comm.RetryCount++;
                                comm.ErrorMessage = "No users found for the specified communication target.";
                                _context.Communications.Update(comm);
                                await _context.SaveChangesAsync(CancellationToken.None);
                                continue;
                            }

                            if (comm.Channel == CommunicationChannel.Email)
                            {
                                foreach (var user in users)
                                {
                                    if (!string.IsNullOrEmpty(user.Email))
                                    {
                                        bool isCommunicationSentToRecipient = await _context.CommunicationRecipients
                                            .AnyAsync(r => r.UserId == user.Id && r.CommunicationId == comm.Id, CancellationToken.None);

                                        if (isCommunicationSentToRecipient)
                                        {
                                            continue;
                                        }

                                        StringBuilder sb = new(comm.Body);
                                        sb.Replace("[Name]", user.FirstName);

                                        EmailMessage emailMessage = new(user.Email, comm.Title ?? "Management Communication", sb.ToString(), null);

                                        bool isEmailSent = await _emailService.SendEmail(emailMessage);

                                        if (isEmailSent)
                                        {
                                            CommunicationRecipient recipient = new()
                                            {
                                                Status = CommunicationRecipientStatus.Success,
                                                UserId = user.Id,
                                                CommunicationId = comm.Id
                                            };

                                            await _context.CommunicationRecipients.AddAsync(recipient, CancellationToken.None);
                                            await _context.SaveChangesAsync(CancellationToken.None);
                                        }
                                        else
                                        {
                                            CommunicationRecipient recipient = new()
                                            {
                                                Status = CommunicationRecipientStatus.Failed,
                                                UserId = user.Id,
                                                CommunicationId = comm.Id,
                                                ErrorMessage = "Failed to send email."
                                            };

                                            await _context.CommunicationRecipients.AddAsync(recipient, CancellationToken.None);
                                            await _context.SaveChangesAsync(CancellationToken.None);
                                        }
                                    }
                                }
                            }
                            else if (comm.Channel == CommunicationChannel.Sms)
                            {
                                continue; // SMS sending logic not yet available
                            }

                            comm.Status = CommunicationStatus.Processed;
                            comm.ProcessedAt = DateTime.UtcNow;

                            _context.Communications.Update(comm);
                            await _context.SaveChangesAsync(CancellationToken.None);
                        }
                        catch (Exception ex)
                        {
                            comm.Status = CommunicationStatus.Failed;
                            comm.ErrorMessage = ex.Message;
                            comm.RetryCount++;
                            _context.Communications.Update(comm);
                            await _context.SaveChangesAsync(CancellationToken.None);
                            _logger.LogError(ex, "Failed to process communication with ID {CommunicationId}", comm.Id);
                            continue;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the communication queue.");
            }
        }

        public async Task ProcessKYCReminderQueue(string userId)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            SqlDbContext _context = scope.ServiceProvider.GetRequiredService<SqlDbContext>();
            IUnitOfWork _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            EmailService _emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

            try
            {
                await _utility.ExecuteWithRetryAsync(async () =>
                {
                    AppUser? user = await _unitOfWork.Users.GetUserByIdMiniAsync(userId, CancellationToken.None);
                    if (user == null)
                    {
                        _logger.LogWarning("User with ID {UserId} not found for KYC reminder.", userId);
                        return;
                    }

                    bool hasPendingKYC = await _unitOfWork.UserManagement.DoesHavPendingKYC(userId, CancellationToken.None);
                    if (!hasPendingKYC)
                    {
                        _logger.LogInformation("User with ID {UserId} does not have pending KYC.", userId);
                        return;
                    }

                    MessageTemplate? messageTemplate = await _unitOfWork.MessageTemplates.GetTemplateByTypeAsync(MessageTemplateType.PendingKYCReminder, CancellationToken.None);
                    if (messageTemplate == null)
                    {
                        _logger.LogWarning("Message template for KYC reminder not found.");
                        return;
                    }

                    StringBuilder sb = new(messageTemplate.Body);
                    sb.Replace("[Name]", user.FirstName);

                    EmailMessage emailMessage = new(user.Email!, "Complete Your Pending KYC", sb.ToString(), null);
                    _ = await _emailService.SendEmail(emailMessage);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the KYC reminder queue.");
            }
        }
    }
}

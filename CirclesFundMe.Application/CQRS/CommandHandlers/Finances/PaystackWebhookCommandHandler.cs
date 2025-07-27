namespace CirclesFundMe.Application.CQRS.CommandHandlers.Finances
{
    public class PaystackWebhookCommandHandler(IUnitOfWork unitOfWork, ILogger<PaystackWebhookCommandHandler> logger, UtilityHelper utility, IOptions<AppSettings> options, IQueueService queueService) : IRequestHandler<PaystackWebhookCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<PaystackWebhookCommandHandler> _logger = logger;
        private readonly UtilityHelper _utility = utility;
        private readonly AppSettings _appSettings = options.Value;
        private readonly IQueueService _queueService = queueService;

        public async Task<BaseResponse<bool>> Handle(PaystackWebhookCommand request, CancellationToken cancellationToken)
        {
            if (request.Data == null)
            {
                _logger.LogError("Paystack webhook data is null for event: {Event}", request.Event);
                return BaseResponse<bool>.BadRequest("Invalid webhook data");
            }

            bool result;
            if (request.Event == "charge.success")
            {
                result = await ProcessChargeSuccessEvent(request.Data, cancellationToken);
            }
            else if (request.Event == "transfer.success")
            {
                result = await ProcessTransferSuccessEvent(request.Data, cancellationToken);
            }
            else
            {
                _logger.LogWarning("Unhandled Paystack webhook event: {Event}", request.Event);
                result = true;
            }

            if (!result)
            {
                _logger.LogError("Failed to process Paystack webhook event with the data: {Data}", request.Data);
            }

            return BaseResponse<bool>.Success(result, "Webhook processed successfully");
        }

        private async Task<bool> ProcessTransferSuccessEvent(object d, CancellationToken cancellationToken)
        {
            TransferWebhookData? data = _utility.Deserializer<TransferWebhookData>(d.ToString()!);
            if (data == null)
            {
                _logger.LogError("Failed to deserialize Paystack webhook transfer data");
                return false;
            }

            if (data.recipient == null || data.recipient.metadata == null)
            {
                _logger.LogError("Recipient or metadata is null in transfer webhook data");
                return false;
            }

            string? currentUserId = data.recipient.metadata.userId?.ToString();
            if (string.IsNullOrEmpty(currentUserId))
            {
                _logger.LogError("User ID not found in transfer metadata for recipient: {Recipient}", data.recipient);
                return false;
            }

            if (data.reference == null)
            {
                _logger.LogError("Transfer reference is null in transfer webhook data for user: {UserId}", currentUserId);
                return false;
            }

            Payment? payment = await _unitOfWork.Payments.GetByPrimaryKey(data.reference, cancellationToken);
            if (payment == null)
            {
                _logger.LogError("Payment not found for reference: {Reference}", data.reference);
                return false;
            }

            if (payment.PaymentStatus == PaymentStatusEnums.Confirmed)
            {
                _logger.LogInformation("Payment already confirmed for reference: {Reference}", data.reference);
                return true;
            }

            Wallet? chargeClearanceWallet = await _unitOfWork.Wallets.GetByPrimaryKey(_appSettings.GLWalletId, cancellationToken);
            if (chargeClearanceWallet == null)
            {
                _logger.LogError("Charge clearance wallet not found with ID: {GLWalletId}", _appSettings.GLWalletId);
                return false;
            }

            Wallet? userContributionWallet = await _unitOfWork.Wallets.GetUserContributionWallet(currentUserId, cancellationToken);
            if (userContributionWallet == null)
            {
                _logger.LogError("User contribution wallet not found for user ID: {UserId}", currentUserId);
                return false;
            }

            string sessionId = UtilityHelper.GenerateRandomUnique30DigitSessionID();

            // Create the debit transaction record
            Transaction debitTransaction = new()
            {
                TransactionReference = data.reference,
                Narration = "Contribution Withdrawal",
                TransactionType = TransactionTypeEnums.Debit,
                BalanceBeforeTransaction = userContributionWallet.Balance,
                Amount = payment.TotalAmount,
                BalanceAfterTransaction = userContributionWallet.Balance - payment.TotalAmount,
                TransactionDate = DateTime.UtcNow,
                TransactionTime = DateTime.UtcNow.TimeOfDay,
                SessionId = sessionId,
                WalletId = userContributionWallet.Id
            };

            // Create the credit transaction record for the charge clearance wallet
            Transaction creditTransaction = new()
            {
                TransactionReference = data.reference,
                Narration = "Contribution Withdrawal Charge",
                TransactionType = TransactionTypeEnums.Credit,
                BalanceBeforeTransaction = chargeClearanceWallet.Balance,
                Amount = payment.ChargeAmount,
                BalanceAfterTransaction = chargeClearanceWallet.Balance + payment.ChargeAmount,
                TransactionDate = DateTime.UtcNow,
                TransactionTime = DateTime.UtcNow.TimeOfDay,
                SessionId = sessionId,
                WalletId = chargeClearanceWallet.Id
            };

            // Update the wallets
            userContributionWallet.Balance -= payment.TotalAmount;
            chargeClearanceWallet.Balance += payment.ChargeAmount;

            // Update Payment
            payment.TransactionDate = data.transferred_at;
            payment.Status = data.status;
            payment.Domain = data.domain;
            payment.GatewayResponse = data.gateway_response;
            payment.Message = data.gateway_response;
            payment.PaymentStatus = data.status switch
            {
                "success" => PaymentStatusEnums.Confirmed,
                "failed" => PaymentStatusEnums.Failed,
                "abandoned" => PaymentStatusEnums.Abandoned,
                _ => PaymentStatusEnums.Awaiting
            };
            payment.ModifiedDate = DateTime.UtcNow;
            payment.ModifiedBy = "PaystackWebhookCommandHandler";

            _unitOfWork.Payments.Update(payment);

            try
            {
                // Add transactions to the unit of work
                await _unitOfWork.Transactions.AddAsync(debitTransaction, cancellationToken);
                await _unitOfWork.Transactions.AddAsync(creditTransaction, cancellationToken);

                // Update wallets in the unit of work
                _unitOfWork.Wallets.Update(userContributionWallet);
                _unitOfWork.Wallets.Update(chargeClearanceWallet);

                // Save changes
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Enqueue notification job
                _queueService.EnqueueFireAndForgetJob<CFMJobs>(j => j.SendNotification(new List<CreateNotificationModel>
                {
                    new()
                    {
                        Title = "Your contribution withdrawal was successful",
                        Type = NotificationTypeEnums.Info,
                        ObjectId = userContributionWallet.UserId,
                        UserId = currentUserId
                    }
                }));

                _queueService.EnqueueFireAndForgetJob<CFMJobs>(j => j.CreateRecentActivity(new RecentActivity
                {
                    Title = "You made a withdrawal",
                    Type = RecentActivityTypeEnums.Withdrawal,
                    Data = payment.ToString(),
                    UserId = currentUserId,
                }));

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing transfer success event for user ID: {UserId} with reference: {Reference}", currentUserId, data.reference);
                return false;
            }
        }

        private async Task<bool> ProcessChargeSuccessEvent(object d, CancellationToken cancellationToken)
        {
            PaymentWebhookData? data = _utility.Deserializer<PaymentWebhookData>(d.ToString()!);
            if (data == null || data.reference == null)
            {
                _logger.LogError("Failed to deserialize Paystack webhook charge data");
                return false;
            }

            string? currentUserId = default;
            if (data.metadata != null)
            {
                currentUserId = data.metadata.userId?.ToString();
            }

            if (string.IsNullOrEmpty(currentUserId))
            {
                _logger.LogError("User ID not found in metadata for user: {Reference}", data.metadata?.userId);
                return false;
            }

            UserContributionScheme? userContributionScheme = await _unitOfWork.UserContributionSchemes.GetOneAsync([ucs => ucs.UserId == currentUserId], cancellationToken);
            if (userContributionScheme == null)
            {
                _logger.LogError("UserContributionScheme not found for user ID: {UserId}", currentUserId);
                return false;
            }

            Payment? payment = await _unitOfWork.Payments.GetByPrimaryKey(data.reference, cancellationToken);
            if (payment == null)
            {
                _logger.LogError("Payment not found for reference: {Reference}", data.reference);
                return false;
            }

            // Update Payment
            payment.Amount = data.amount / 100;
            payment.TransactionDate = data.paid_at;
            payment.Status = data.status;
            payment.Domain = data.domain;
            payment.GatewayResponse = data.gateway_response;
            payment.Message = data.message;
            payment.Channel = data.channel;
            payment.IpAddress = data.ip_address;
            payment.AuthorizationCode = data.authorization?.authorization_code;
            payment.PaymentStatus = data.status switch
            {
                "success" => PaymentStatusEnums.Confirmed,
                "failed" => PaymentStatusEnums.Failed,
                "abandoned" => PaymentStatusEnums.Abandoned,
                _ => PaymentStatusEnums.Awaiting
            };
            payment.ModifiedDate = DateTime.UtcNow;
            payment.ModifiedBy = "PaystackWebhookCommandHandler";

            _unitOfWork.Payments.Update(payment);

            // Link Card
            LinkedCard? linkedCard = await _unitOfWork.LinkedCards.GetOneAsync([lc => lc.UserId == currentUserId], cancellationToken);
            if (linkedCard == null)
            {
                linkedCard = new LinkedCard
                {
                    AuthorizationCode = data.authorization?.authorization_code,
                    Last4Digits = data.authorization?.last4,
                    CardType = data.authorization?.card_type,
                    ExpiryMonth = data.authorization?.exp_month,
                    ExpiryYear = data.authorization?.exp_year,
                    Bin = data.authorization?.bin,
                    UserId = currentUserId,
                };
                await _unitOfWork.LinkedCards.AddAsync(linkedCard, cancellationToken);
            }
            else
            {
                linkedCard.AuthorizationCode = data.authorization?.authorization_code;
                linkedCard.Last4Digits = data.authorization?.last4;
                linkedCard.CardType = data.authorization?.card_type;
                linkedCard.ExpiryMonth = data.authorization?.exp_month;
                linkedCard.ExpiryYear = data.authorization?.exp_year;
                linkedCard.Bin = data.authorization?.bin;

                _unitOfWork.LinkedCards.Update(linkedCard);
            }

            decimal amountToCreditUser = payment.Amount - userContributionScheme.ChargeAmount;

            await _unitOfWork.UserContributions.AddAsync(new UserContribution
            {
                Amount = amountToCreditUser,
                AmountIncludingCharges = payment.Amount,
                UserId = currentUserId
            }, cancellationToken);

            // Get User Wallet
            Wallet? wallet = await _unitOfWork.Wallets.GetOneAsync([w => w.UserId == currentUserId, w => w.Type == WalletTypeEnums.Contribution], cancellationToken);
            if (wallet == null)
            {
                _logger.LogError("Wallet not found for user ID: {UserId}", currentUserId);
                return false;
            }

            // Create Transaction
            Transaction transaction = new()
            {
                TransactionReference = data.reference,
                Narration = $"Contribution",
                TransactionType = TransactionTypeEnums.Credit,
                BalanceBeforeTransaction = wallet.Balance,
                Amount = amountToCreditUser,
                BalanceAfterTransaction = wallet.Balance + amountToCreditUser,
                TransactionDate = DateTime.UtcNow,
                TransactionTime = DateTime.UtcNow.TimeOfDay,
                WalletId = wallet.Id,
            };

            await _unitOfWork.Transactions.AddAsync(transaction, cancellationToken);

            wallet.Balance += amountToCreditUser;
            _unitOfWork.Wallets.Update(wallet);

            // Get Collection Wallet
            Wallet? collectionWallet = await _unitOfWork.Wallets.GetByPrimaryKey(_appSettings.GLWalletId, cancellationToken);
            if (collectionWallet == null)
            {
                _logger.LogError("Collection wallet not found with ID: {GLWalletId}", _appSettings.GLWalletId);
                return false;
            }

            // Create Collection Transaction
            Transaction collectionTransaction = new()
            {
                TransactionReference = data.reference,
                Narration = $"Contribution Charge",
                TransactionType = TransactionTypeEnums.Credit,
                BalanceBeforeTransaction = collectionWallet.Balance,
                Amount = userContributionScheme.ChargeAmount,
                BalanceAfterTransaction = collectionWallet.Balance + userContributionScheme.ChargeAmount,
                TransactionDate = DateTime.UtcNow,
                TransactionTime = DateTime.UtcNow.TimeOfDay,
                WalletId = collectionWallet.Id,
            };

            await _unitOfWork.Transactions.AddAsync(collectionTransaction, cancellationToken);

            collectionWallet.Balance += userContributionScheme.ChargeAmount;
            _unitOfWork.Wallets.Update(collectionWallet);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _queueService.EnqueueFireAndForgetJob<CFMJobs>(j => j.SendNotification(new List<CreateNotificationModel>
            {
                new()
                {
                    Title = "Your contribution was successful",
                    Type = NotificationTypeEnums.Info,
                    ObjectId = wallet.UserId,
                    UserId = currentUserId
                }
            }));

            _queueService.EnqueueFireAndForgetJob<CFMJobs>(j => j.CreateRecentActivity(new RecentActivity
            {
                Title = "You made a contribution",
                Type = RecentActivityTypeEnums.Contribution,
                Data = payment.ToString(),
                UserId = currentUserId,
            }));

            return true;
        }
    }
}

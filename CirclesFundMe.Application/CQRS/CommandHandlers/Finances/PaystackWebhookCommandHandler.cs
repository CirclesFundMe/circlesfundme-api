namespace CirclesFundMe.Application.CQRS.CommandHandlers.Finances
{
    public class PaystackWebhookCommandHandler(IUnitOfWork unitOfWork, ILogger<PaystackWebhookCommandHandler> logger, UtilityHelper utility, IOptions<AppSettings> options) : IRequestHandler<PaystackWebhookCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<PaystackWebhookCommandHandler> _logger = logger;
        private readonly UtilityHelper _utility = utility;
        private readonly AppSettings _appSettings = options.Value;

        public async Task<BaseResponse<bool>> Handle(PaystackWebhookCommand request, CancellationToken cancellationToken)
        {
            if (request.Event != "charge.success")
            {
                _logger.LogInformation("Received Paystack webhook event: {Event}", request.Event);
                return BaseResponse<bool>.Success(true, "Event not handled");
            }

            if (request.Data == null)
            {
                _logger.LogError("Paystack webhook data is null for event: {Event}", request.Event);
                return BaseResponse<bool>.BadRequest("Invalid webhook data");
            }

            PaymentWebhookData? data = _utility.Deserializer<PaymentWebhookData>(request.Data.ToString()!);
            if (data == null || data.reference == null)
            {
                _logger.LogError("Failed to deserialize Paystack webhook data for event: {Event}", request.Event);
                return BaseResponse<bool>.BadRequest("Invalid webhook data format");
            }

            // Get Metadata
            string? currentUserId = default;
            if (data.metadata != null)
            {
                // metadata was stored as `new { userId = user.Id }` in the InitializeTransactionPayload
                currentUserId = data.metadata.userId?.ToString();
            }

            if (string.IsNullOrEmpty(currentUserId))
            {
                _logger.LogError("User ID not found in metadata for reference: {Reference}", data.reference);
                return BaseResponse<bool>.BadRequest("User ID not found in metadata");
            }

            UserContributionScheme? userContributionScheme = await _unitOfWork.UserContributionSchemes.GetOneAsync([ucs => ucs.UserId == currentUserId], cancellationToken);
            if (userContributionScheme == null)
            {
                _logger.LogError("UserContributionScheme not found for user ID: {UserId}", currentUserId);
                return BaseResponse<bool>.NotFound("UserContributionScheme not found");
            }

            Payment? payment = await _unitOfWork.Payments.GetByPrimaryKey(data.reference, cancellationToken);
            if (payment == null)
            {
                _logger.LogError("Payment not found for reference: {Reference}", data.reference);
                return BaseResponse<bool>.NotFound("Payment not found");
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
                return BaseResponse<bool>.NotFound("Wallet not found");
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
                return BaseResponse<bool>.NotFound("Collection wallet not found");
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

            bool isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isSaved)
            {
                _logger.LogError("Failed to save payment and wallet updates for reference: {Reference}", data.reference);
                return BaseResponse<bool>.BadRequest("Failed to save payment and wallet updates");
            }

            return new BaseResponse<bool>
            {
                Message = "Payment processed successfully",
                Data = true
            };
        }
    }
}

namespace CirclesFundMe.Application.CQRS.CommandHandlers.Finances
{
    public class PaystackWebhookCommandHandler(IUnitOfWork unitOfWork, ILogger<PaystackWebhookCommandHandler> logger, UtilityHelper utility) : IRequestHandler<PaystackWebhookCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<PaystackWebhookCommandHandler> _logger = logger;
        private readonly UtilityHelper _utility = utility;

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
                Dictionary<string, string>? metadata = _utility.Deserializer<Dictionary<string, string>>(data.metadata.ToString()!);
                if (metadata != null && metadata.TryGetValue("userId", out string? userId) && !string.IsNullOrEmpty(userId))
                {
                    currentUserId = userId;
                }
            }

            if (string.IsNullOrEmpty(currentUserId))
            {
                _logger.LogError("User ID not found in metadata for reference: {Reference}", data.reference);
                return BaseResponse<bool>.BadRequest("User ID not found in metadata");
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

            // Get Wallet
            Wallet? wallet = await _unitOfWork.Wallets.GetOneAsync([w => w.UserId == currentUserId], cancellationToken);
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
                Amount = payment.Amount,
                BalanceAfterTransaction = wallet.Balance + payment.Amount,
                TransactionDate = DateTime.UtcNow,
                TransactionTime = DateTime.UtcNow.TimeOfDay,
                WalletId = wallet.Id,
            };

            await _unitOfWork.Transactions.AddAsync(transaction, cancellationToken);

            wallet.Balance += payment.Amount;
            _unitOfWork.Wallets.Update(wallet);

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

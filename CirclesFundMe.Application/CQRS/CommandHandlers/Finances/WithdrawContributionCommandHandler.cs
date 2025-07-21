
namespace CirclesFundMe.Application.CQRS.CommandHandlers.Finances
{
    public class WithdrawContributionCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IPaystackClient paystackClient, IOptions<AppSettings> options, IQueueService queueService) : IRequestHandler<WithdrawContributionCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IPaystackClient _paystackClient = paystackClient;
        private readonly AppSettings _appSettings = options.Value;
        private readonly IQueueService _queueService = queueService;

        public async Task<BaseResponse<bool>> Handle(WithdrawContributionCommand request, CancellationToken cancellationToken)
        {
            UserWithdrawalSetting? userWithdrawalSetting = await _unitOfWork.UserWithdrawalSettings.GetOneAsync([w => w.UserId == _currentUserService.UserId], cancellationToken);

            if (userWithdrawalSetting == null)
            {
                return BaseResponse<bool>.BadRequest("You do not have a withdrawal setting. Please create one first.");
            }

            Wallet? chargeClearanceWallet = await _unitOfWork.Wallets.GetByPrimaryKey(_appSettings.GLWalletId, cancellationToken);
            if (chargeClearanceWallet == null)
            {
                return BaseResponse<bool>.BadRequest("Charge wallet not found. Please contact support.");
            }

            Wallet? userContributionWallet = await _unitOfWork.Wallets.GetUserContributionWallet(_currentUserService.UserId, cancellationToken);
            if (userContributionWallet == null)
            {
                return BaseResponse<bool>.BadRequest("You do not have a contribution wallet. Please contact support.");
            }

            decimal withdrawalCharge = _appSettings.WithdrawalCharge;

            decimal amountToSendCustomer = request.DeductChargeFromBalance
                ? request.Amount
                : request.Amount - withdrawalCharge;

            decimal totalAmountToWithdraw = request.DeductChargeFromBalance
                ? request.Amount + withdrawalCharge
                : request.Amount;

            if (userContributionWallet.Balance < totalAmountToWithdraw)
            {
                return BaseResponse<bool>.BadRequest("Insufficient funds in your contribution wallet.");
            }

            string sessionId = UtilityHelper.GenerateRandomUnique30DigitSessionID();

            BasePaystackResponse<TransferFundData> transferResponse = await _paystackClient.TransferFund(new TransferFundPayload
            {
                Amount = amountToSendCustomer * 100, // In Kobo
                Recipient = userWithdrawalSetting.PaystackRecipientCode,
                Reason = "Contribution Withdrawal",
                Reference = Guid.NewGuid().ToString("N")
            }, cancellationToken);

            if (transferResponse.Status == false || transferResponse.Data == null)
            {
                return BaseResponse<bool>.BadRequest("Unable to process withdrawal. Please try again later.");
            }

            // Create the debit transaction record
            Transaction debitTransaction = new()
            {
                TransactionReference = transferResponse.Data.Reference,
                Narration = "Contribution Withdrawal",
                TransactionType = TransactionTypeEnums.Debit,
                BalanceBeforeTransaction = userContributionWallet.Balance,
                Amount = totalAmountToWithdraw,
                BalanceAfterTransaction = userContributionWallet.Balance - totalAmountToWithdraw,
                TransactionDate = DateTime.UtcNow,
                TransactionTime = DateTime.UtcNow.TimeOfDay,
                SessionId = sessionId,
                WalletId = userContributionWallet.Id
            };

            // Create the credit transaction record for the charge clearance wallet
            Transaction creditTransaction = new()
            {
                TransactionReference = transferResponse.Data.Reference,
                Narration = "Contribution Withdrawal Charge",
                TransactionType = TransactionTypeEnums.Credit,
                BalanceBeforeTransaction = chargeClearanceWallet.Balance,
                Amount = withdrawalCharge,
                BalanceAfterTransaction = chargeClearanceWallet.Balance + withdrawalCharge,
                TransactionDate = DateTime.UtcNow,
                TransactionTime = DateTime.UtcNow.TimeOfDay,
                SessionId = sessionId,
                WalletId = chargeClearanceWallet.Id
            };

            // Update the wallets
            userContributionWallet.Balance -= totalAmountToWithdraw;
            chargeClearanceWallet.Balance += withdrawalCharge;

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
                        UserId = _currentUserService.UserId
                    }
                }));

                return BaseResponse<bool>.Success(true, "Withdrawal processed successfully.");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.BadRequest($"An error occurred while processing your withdrawal: {ex.Message}");
            }
        }
    }
}

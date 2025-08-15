
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

            decimal withdrawalCharge = 0;
            if (_appSettings.EnableWithdrawalCharge)
            {
                withdrawalCharge = _appSettings.WithdrawalCharge;
            }

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

            TransferFundPayload payload = new()
            {
                amount = amountToSendCustomer * 100, // In Kobo
                recipient = userWithdrawalSetting.PaystackRecipientCode,
                reason = "Contribution Withdrawal",
                reference = Guid.NewGuid().ToString("N"),
                metadata = new MetaDataObj
                {
                    userId = _currentUserService.UserId,
                    amount_with_charge = totalAmountToWithdraw.ToString()
                }
            };

            Payment payment = new()
            {
                Reference = payload.reference,
                Amount = payload.amount / 100, // Convert back to Naira
                ChargeAmount = withdrawalCharge,
                TotalAmount = totalAmountToWithdraw,
                Currency = payload.currency ?? "NGN",
                PaymentStatus = PaymentStatusEnums.Awaiting,
                PaymentType = PaymentTypeEnums.Outflow,
                UserId = _currentUserService.UserId,
            };

            payment.BasicValidate();

            await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            BasePaystackResponse<TransferFundData> transferResponse = await _paystackClient.TransferFund(payload, cancellationToken);

            if (transferResponse.status == false)
            {
                return BaseResponse<bool>.BadRequest("Unable to process withdrawal");
            }
            else
            {
                return BaseResponse<bool>.Success(true, "Withdrawal request processed successfully");
            }
        }
    }
}

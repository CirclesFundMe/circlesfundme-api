
namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class UpdateWithdrawalSettingCommandHandler(IUnitOfWork unitOfWork, IPaystackClient paystackClient, ILogger<UpdateWithdrawalSettingCommandHandler> logger, IOTPService oTPService, ICurrentUserService currentUserService) : IRequestHandler<UpdateWithdrawalSettingCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPaystackClient _paystackClient = paystackClient;
        private readonly ILogger<UpdateWithdrawalSettingCommandHandler> _logger = logger;
        private readonly IOTPService _oTPService = oTPService;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<bool>> Handle(UpdateWithdrawalSettingCommand request, CancellationToken cancellationToken)
        {
            UserWithdrawalSetting? withdrawalSetting = await _unitOfWork.UserWithdrawalSettings.GetByPrimaryKey(request.WithdrawalSettingId, cancellationToken);
            if (withdrawalSetting == null)
            {
                return BaseResponse<bool>.NotFound("Withdrawal setting not found.");
            }

            (bool otpValid, string message) = await _oTPService.ValidateOtp(_currentUserService.UserEmail, request.Otp!, cancellationToken);
            if (!otpValid)
            {
                return BaseResponse<bool>.BadRequest(message);
            }

            if (!string.IsNullOrEmpty(request.AccountNumber) && !string.IsNullOrEmpty(request.BankCode))
            {
                Bank? bank = await _unitOfWork.Banks.GetByPrimaryKey(request.BankCode, cancellationToken);
                if (bank == null)
                {
                    return BaseResponse<bool>.BadRequest("Invalid bank code.");
                }

                BasePaystackResponse<VerifyAccountNumberData> accountVerificationResponse = await _paystackClient.VerifyAccountNumberData(new VerifyAccountNumberQuery
                {
                    AccountNumber = request.AccountNumber,
                    BankCode = bank.Code
                }, cancellationToken);

                if (accountVerificationResponse.Status == false || accountVerificationResponse.Data == null)
                {
                    return BaseResponse<bool>.BadRequest("Unable to verify account number.");
                }

                await _paystackClient.DeleteTransferRecipient(withdrawalSetting.PaystackRecipientCode!, cancellationToken);

                BasePaystackResponse<AddRecipientData> res = await _paystackClient.AddTransferRecipient(new AddTransferRecipientPayload
                {
                    Name = accountVerificationResponse.Data.AccountName,
                    AccountNumber = accountVerificationResponse.Data.AccountNumber,
                    BankCode = bank.Code
                }, cancellationToken);

                if (res.Status == false || res.Data == null || res.Data.RecipientCode == null)
                {
                    return BaseResponse<bool>.BadRequest("Unable to add transfer recipient.");
                }

                withdrawalSetting.AccountNumber = accountVerificationResponse.Data.AccountNumber;
                withdrawalSetting.AccountName = accountVerificationResponse.Data.AccountName;
                withdrawalSetting.BankCode = bank.Code;
                withdrawalSetting.PaystackRecipientCode = res.Data.RecipientCode;

                try
                {
                    _unitOfWork.UserWithdrawalSettings.Update(withdrawalSetting);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return BaseResponse<bool>.Success(true, "Withdrawal setting updated successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the withdrawal setting for User ID: {UserId}", withdrawalSetting.UserId);

                    await _paystackClient.DeleteTransferRecipient(res.Data.RecipientCode, cancellationToken);

                    return BaseResponse<bool>.BadRequest($"An error occurred while updating the withdrawal setting");
                }
            }

            return BaseResponse<bool>.Success(true, "No changes made to your withdrawal settings");
        }
    }
}

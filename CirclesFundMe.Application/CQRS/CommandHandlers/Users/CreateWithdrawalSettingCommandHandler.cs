namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class CreateWithdrawalSettingCommandHandler(IUnitOfWork unitOfWork, IPaystackClient paystackClients, ICurrentUserService currentUserService, ILogger<CreateWithdrawalSettingCommandHandler> logger, IQueueService queueService) : IRequestHandler<CreateWithdrawalSettingCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPaystackClient _paystackClients = paystackClients;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ILogger<CreateWithdrawalSettingCommandHandler> _logger = logger;
        private readonly IQueueService _queueService = queueService;

        public async Task<BaseResponse<bool>> Handle(CreateWithdrawalSettingCommand request, CancellationToken cancellationToken)
        {
            bool withdrawalSettingExists = await _unitOfWork.UserWithdrawalSettings.ExistAsync([x => x.UserId == _currentUserService.UserId], cancellationToken);

            if (withdrawalSettingExists)
            {
                return BaseResponse<bool>.BadRequest("You already have a withdrawal setting.");
            }

            Bank? bank = await _unitOfWork.Banks.GetByPrimaryKey(request.BankCode!, cancellationToken);
            if (bank == null)
            {
                return BaseResponse<bool>.BadRequest("Invalid bank code.");
            }

            BasePaystackResponse<VerifyAccountNumberData> accountVerificationResponse = await _paystackClients.VerifyAccountNumberData(new VerifyAccountNumberQuery
            {
                AccountNumber = request.AccountNumber!,
                BankCode = request.BankCode!
            }, cancellationToken);

            if (accountVerificationResponse.Status == false || accountVerificationResponse.Data == null)
            {
                return BaseResponse<bool>.BadRequest("Unable to verify account number");
            }

            BasePaystackResponse<AddRecipientData> res = await _paystackClients.AddTransferRecipient(new AddTransferRecipientPayload
            {
                Name = accountVerificationResponse.Data.AccountName,
                AccountNumber = accountVerificationResponse.Data.AccountNumber,
                BankCode = bank.Code
            }, cancellationToken);

            if (res.Status == false || res.Data == null || res.Data.RecipientCode == null)
            {
                return BaseResponse<bool>.BadRequest("Unable to add transfer recipient");
            }

            UserWithdrawalSetting withdrawalSetting = new()
            {
                PaystackRecipientCode = res.Data.RecipientCode,
                AccountNumber = accountVerificationResponse.Data.AccountNumber,
                AccountName = accountVerificationResponse.Data.AccountName,
                BankCode = bank.Code,
                UserId = _currentUserService.UserId
            };

            try
            {
                await _unitOfWork.UserWithdrawalSettings.AddAsync(withdrawalSetting, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _queueService.EnqueueFireAndForgetJob<CFMJobs>(j => j.SendNotification(new List<CreateNotificationModel>
                {
                    new()
                    {
                        Title = "Your withdrawal account setup was successful",
                        Type = NotificationTypeEnums.Info,
                        ObjectId = withdrawalSetting.UserId,
                        UserId = _currentUserService.UserId
                    }
                }));

                return BaseResponse<bool>.Success(true, "Withdrawal setting created successfully.");
            }
            catch (Exception ex)
            {
                await _paystackClients.DeleteTransferRecipient(res.Data.RecipientCode, cancellationToken);
                _logger.LogError(ex, "Error creating withdrawal setting for user {UserId}", _currentUserService.UserId);
                return BaseResponse<bool>.BadRequest($"An error occurred while creating withdrawal setting");
            }
        }
    }
}

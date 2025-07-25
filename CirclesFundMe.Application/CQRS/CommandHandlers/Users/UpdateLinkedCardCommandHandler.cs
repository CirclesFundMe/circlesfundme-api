namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class UpdateLinkedCardCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IPaystackClient paystackClient, IOTPService oTPService) : IRequestHandler<UpdateLinkedCardCommand, BaseResponse<InitializeTransactionModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IPaystackClient _paystackClient = paystackClient;
        private readonly IOTPService _oTPService = oTPService;

        public async Task<BaseResponse<InitializeTransactionModel>> Handle(UpdateLinkedCardCommand request, CancellationToken cancellationToken)
        {
            AppUserExtension? user = await _unitOfWork.Users.GetUserByIdAsync(_currentUserService.UserId, cancellationToken);
            if (user == null)
            {
                return BaseResponse<InitializeTransactionModel>.NotFound("User not found");
            }

            (bool otpValid, string message) = await _oTPService.ValidateOtp(_currentUserService.UserEmail, request.Otp, cancellationToken);
            if (!otpValid)
            {
                return BaseResponse<InitializeTransactionModel>.BadRequest(message);
            }

            if (!user.IsCardLinked)
            {
                return BaseResponse<InitializeTransactionModel>.BadRequest("No card linked to this user. Please link a card first.");
            }

            decimal amountToContribute = (user.UserContributionScheme?.ContributionAmount ?? 0) * 100; // In Kobo
            InitializeTransactionPayload payload = new()
            {
                Email = user.Email,
                Amount = amountToContribute,
                Reference = Guid.NewGuid().ToString("N"),
                Metadata = new MetaDataObj
                {
                    userId = user.Id,
                    updateCard = true
                }
            };

            BasePaystackResponse<InitializeTransactionData> initializeTransaction = await _paystackClient.InitializeTransaction(payload, cancellationToken);
            if (!initializeTransaction.status || initializeTransaction.data == null)
            {
                return BaseResponse<InitializeTransactionModel>.BadRequest(initializeTransaction.message ?? "Error while initializing payment");
            }

            Payment payment = new()
            {
                AccessCode = initializeTransaction.data.access_code,
                AuthorizationUrl = initializeTransaction.data.authorization_url,
                Reference = initializeTransaction.data.reference,
                Amount = amountToContribute / 100, // Convert back to Naira
                Currency = payload.Currency ?? "NGN",
                PaymentStatus = PaymentStatusEnums.Awaiting,
                UserId = user.Id,
            };

            await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
            bool isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isSaved)
            {
                return BaseResponse<InitializeTransactionModel>.BadRequest("Failed to save payment details");
            }

            return new()
            {
                Message = "Payment initialized successfully",
                Data = new InitializeTransactionModel
                {
                    AuthorizationUrl = payment.AuthorizationUrl,
                    Reference = payment.Reference
                },
            };
        }
    }
}

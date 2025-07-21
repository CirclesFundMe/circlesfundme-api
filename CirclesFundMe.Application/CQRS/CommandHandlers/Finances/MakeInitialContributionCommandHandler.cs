
namespace CirclesFundMe.Application.CQRS.CommandHandlers.Finances
{
    public class MakeInitialContributionCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IPaystackClient paystackClient) : IRequestHandler<MakeInitialContributionCommand, BaseResponse<InitializeTransactionModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IPaystackClient _paystackClient = paystackClient;

        public async Task<BaseResponse<InitializeTransactionModel>> Handle(MakeInitialContributionCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _unitOfWork.Users.GetUserByIdAsync(_currentUserService.UserId, cancellationToken);
            if (user == null)
            {
                return BaseResponse<InitializeTransactionModel>.NotFound("User not found");
            }

            decimal amountToContribute = (user.UserContributionScheme?.ContributionAmount ?? 0) * 100; // In Kobo
            InitializeTransactionPayload payload = new()
            {
                Email = user.Email,
                Amount = amountToContribute,
                Reference = Guid.NewGuid().ToString("N")
            };

            BasePaystackResponse<InitializeTransactionData> initializeTransaction = await _paystackClient.InitializeTransaction(payload, cancellationToken);
            if (!initializeTransaction.Status || initializeTransaction.Data == null)
            {
                return BaseResponse<InitializeTransactionModel>.BadRequest(initializeTransaction.Message ?? "Error while initializing payment");
            }

            Payment payment = new()
            {
                AccessCode = initializeTransaction.Data.AccessCode,
                AuthorizationUrl = initializeTransaction.Data.AuthorizationUrl,
                Reference = initializeTransaction.Data.Reference,
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

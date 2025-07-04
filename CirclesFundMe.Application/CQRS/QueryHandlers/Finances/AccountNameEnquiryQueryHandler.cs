namespace CirclesFundMe.Application.CQRS.QueryHandlers.Finances
{
    public class AccountNameEnquiryQueryHandler(IPaystackClient paystackClient, IConfiguration config) : IRequestHandler<AccountNameEnquiryQuery, BaseResponse<string>>
    {
        private readonly IPaystackClient _paystackClient = paystackClient;
        private readonly bool _isPaystackEnabled = config.GetValue<bool>("PaystackService:IsEnabled");

        public async Task<BaseResponse<string>> Handle(AccountNameEnquiryQuery request, CancellationToken cancellationToken)
        {
            if (_isPaystackEnabled)
            {
                BasePaystackResponse<VerifyAccountNumberData> res = await _paystackClient.VerifyAccountNumberData(new VerifyAccountNumberQuery
                {
                    AccountNumber = request.AccountNumber,
                    BankCode = request.BankCode
                }, cancellationToken);

                if (!res.Status || res.Data == null || res.Data.AccountName == null)
                    return BaseResponse<string>.BadRequest("Unable to verify account number");

                return BaseResponse<string>.Success(res.Data.AccountName, "Account name retrieved successfully");
            }
            else
            {
                return BaseResponse<string>.Success("Test Account Name");
            }
        }
    }
}

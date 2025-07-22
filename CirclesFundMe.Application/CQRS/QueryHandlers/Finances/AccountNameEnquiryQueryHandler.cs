namespace CirclesFundMe.Application.CQRS.QueryHandlers.Finances
{
    public class AccountNameEnquiryQueryHandler(IPaystackClient paystackClient) : IRequestHandler<AccountNameEnquiryQuery, BaseResponse<string>>
    {
        private readonly IPaystackClient _paystackClient = paystackClient;

        public async Task<BaseResponse<string>> Handle(AccountNameEnquiryQuery request, CancellationToken cancellationToken)
        {
            BasePaystackResponse<VerifyAccountNumberData> res = await _paystackClient.VerifyAccountNumberData(new VerifyAccountNumberQuery
            {
                AccountNumber = request.AccountNumber,
                BankCode = request.BankCode
            }, cancellationToken);

            if (!res.status || res.data == null || res.data.account_name == null)
                return BaseResponse<string>.BadRequest("Unable to verify account number");

            return BaseResponse<string>.Success(res.data.account_name, "Account name retrieved successfully");
        }
    }
}

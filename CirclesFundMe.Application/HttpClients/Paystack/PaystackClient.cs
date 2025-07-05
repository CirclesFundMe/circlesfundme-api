namespace CirclesFundMe.Application.HttpClients.Paystack
{
    public interface IPaystackClient
    {
        Task<BasePaystackResponse<List<BankData>>> GetBanksList(BankDataQuery query, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<VerifyAccountNumberData>> VerifyAccountNumberData(VerifyAccountNumberQuery query, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<AddRecipientData>> AddTransferRecipient(AddTransferRecipientPayload payload, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<Unit>> DeleteTransferRecipient(string recipientCode, CancellationToken cancellationToken = default);
    }

    public class PaystackClient(IHttpClientFactory factory, ILogger<PaystackClient> logger) : IPaystackClient
    {
        private readonly ILogger<PaystackClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly RestClientService _restClientService = new(factory.CreateClient("PaystackService"), logger);

        public async Task<BasePaystackResponse<AddRecipientData>> AddTransferRecipient(AddTransferRecipientPayload payload, CancellationToken cancellationToken = default)
        {
            string uri = "transferrecipient";

            try
            {
                StringContent content = new(UtilityHelper.Serializer(payload), Encoding.UTF8, "application/json");

                BasePaystackResponse<AddRecipientData>? response = await _restClientService.PostAsync<BasePaystackResponse<AddRecipientData>>(uri, content, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<AddRecipientData>
                    {
                        Status = false,
                        Message = "No data received for transfer recipient Paystack",
                        Data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<AddRecipientData>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BasePaystackResponse<Unit>> DeleteTransferRecipient(string recipientCode, CancellationToken cancellationToken = default)
        {
            string uri = $"transferrecipient/{recipientCode}";

            try
            {
                bool isDeleted = await _restClientService.DeleteAsync(uri, cancellationToken);

                if (!isDeleted)
                {
                    return new BasePaystackResponse<Unit>
                    {
                        Status = false,
                        Message = "Failed to delete transfer recipient",
                        Data = Unit.Value
                    };
                }

                return new BasePaystackResponse<Unit>
                {
                    Status = true,
                    Message = "Transfer recipient deleted successfully",
                    Data = Unit.Value
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<Unit>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = Unit.Value
                };
            }
        }

        public async Task<BasePaystackResponse<List<BankData>>> GetBanksList(BankDataQuery query, CancellationToken cancellationToken = default)
        {
            string uri = $"bank?country=nigeria&perPage={query.PerPage}&use_cursor=true";
            if (query.UseCursor && !string.IsNullOrEmpty(query.Next))
            {
                uri += $"&next={query.Next}";
            }

            try
            {
                var response = await _restClientService.GetAsync<BasePaystackResponse<List<BankData>>>(uri, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<List<BankData>>
                    {
                        Status = false,
                        Message = "No data received for bank list Paystack",
                        Data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<List<BankData>>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BasePaystackResponse<VerifyAccountNumberData>> VerifyAccountNumberData(VerifyAccountNumberQuery query, CancellationToken cancellationToken = default)
        {
            string uri = $"bank/resolve?account_number={query.AccountNumber}&bank_code={query.BankCode}";

            try
            {
                var response = await _restClientService.GetAsync<BasePaystackResponse<VerifyAccountNumberData>>(uri, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<VerifyAccountNumberData>
                    {
                        Status = false,
                        Message = "No data received for account verification from Paystack",
                        Data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<VerifyAccountNumberData>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}

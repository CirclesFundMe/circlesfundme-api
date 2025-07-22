namespace CirclesFundMe.Application.HttpClients.Paystack
{
    public interface IPaystackClient
    {
        Task<BasePaystackResponse<List<BankData>>> GetBanksList(BankDataQuery query, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<VerifyAccountNumberData>> VerifyAccountNumberData(VerifyAccountNumberQuery query, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<AddRecipientData>> AddTransferRecipient(AddTransferRecipientPayload payload, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<Unit>> DeleteTransferRecipient(string recipientCode, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<InitializeTransactionData>> InitializeTransaction(InitializeTransactionPayload payload, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<VerifyTransactionData>> VerifyTransaction(string reference, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<TransferFundData>> TransferFund(TransferFundPayload payload, CancellationToken cancellationToken = default);

        Task<BasePaystackResponse<ChargeAuthorizationData>> ChargeAuthorization(ChargeAuthorizationPayload payload, CancellationToken cancellationToken = default);
    }

    public class PaystackClient(IHttpClientFactory factory, ILogger<PaystackClient> logger, IConfiguration config) : IPaystackClient
    {
        private readonly ILogger<PaystackClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly RestClientService _restClientService = new(factory.CreateClient("PaystackService"), logger);
        private readonly bool _isPaystackEnabled = config.GetValue<bool>("PaystackService:IsEnabled");

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
                        status = false,
                        message = "No data received for transfer recipient Paystack",
                        data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<AddRecipientData>
                {
                    status = false,
                    message = ex.Message,
                    data = null
                };
            }
        }

        public async Task<BasePaystackResponse<ChargeAuthorizationData>> ChargeAuthorization(ChargeAuthorizationPayload payload, CancellationToken cancellationToken = default)
        {
            string uri = "transaction/charge_authorization";

            try
            {
                StringContent content = new(UtilityHelper.Serializer(payload), Encoding.UTF8, "application/json");

                BasePaystackResponse<ChargeAuthorizationData>? response = await _restClientService.PostAsync<BasePaystackResponse<ChargeAuthorizationData>>(uri, content, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<ChargeAuthorizationData>
                    {
                        status = false,
                        message = "No data received for authorization charge Paystack",
                        data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<ChargeAuthorizationData>
                {
                    status = false,
                    message = ex.Message,
                    data = null
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
                        status = false,
                        message = "Failed to delete transfer recipient",
                        data = Unit.Value
                    };
                }

                return new BasePaystackResponse<Unit>
                {
                    status = true,
                    message = "Transfer recipient deleted successfully",
                    data = Unit.Value
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<Unit>
                {
                    status = false,
                    message = ex.Message,
                    data = Unit.Value
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
                        status = false,
                        message = "No data received for bank list Paystack",
                        data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<List<BankData>>
                {
                    status = false,
                    message = ex.Message,
                    data = null
                };
            }
        }

        public async Task<BasePaystackResponse<InitializeTransactionData>> InitializeTransaction(InitializeTransactionPayload payload, CancellationToken cancellationToken = default)
        {
            string uri = "transaction/initialize";

            try
            {
                StringContent content = new(UtilityHelper.Serializer(payload), Encoding.UTF8, "application/json");

                var response = await _restClientService.PostAsync<BasePaystackResponse<InitializeTransactionData>>(uri, content, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<InitializeTransactionData>
                    {
                        status = false,
                        message = "No data received for transaction initialization from Paystack",
                        data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<InitializeTransactionData>
                {
                    status = false,
                    message = ex.Message,
                    data = null
                };
            }
        }

        public async Task<BasePaystackResponse<TransferFundData>> TransferFund(TransferFundPayload payload, CancellationToken cancellationToken = default)
        {
            string uri = "transfer";

            try
            {
                StringContent content = new(UtilityHelper.Serializer(payload), Encoding.UTF8, "application/json");

                var response = await _restClientService.PostAsync<BasePaystackResponse<TransferFundData>>(uri, content, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<TransferFundData>
                    {
                        status = false,
                        message = "No data received for fund transfer from Paystack",
                        data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<TransferFundData>
                {
                    status = false,
                    message = ex.Message,
                    data = null
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
                        status = false,
                        message = "No data received for account verification from Paystack",
                        data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<VerifyAccountNumberData>
                {
                    status = false,
                    message = ex.Message,
                    data = null
                };
            }
        }

        public async Task<BasePaystackResponse<VerifyTransactionData>> VerifyTransaction(string reference, CancellationToken cancellationToken = default)
        {
            string uri = $"transaction/verify/{reference}";

            try
            {
                var response = await _restClientService.GetAsync<BasePaystackResponse<VerifyTransactionData>>(uri, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<VerifyTransactionData>
                    {
                        status = false,
                        message = "No data received for transaction verification from Paystack",
                        data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<VerifyTransactionData>
                {
                    status = false,
                    message = ex.Message,
                    data = null
                };
            }
        }
    }
}

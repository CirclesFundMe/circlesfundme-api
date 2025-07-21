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
            if (!_isPaystackEnabled)
            {
                return new BasePaystackResponse<AddRecipientData>
                {
                    Status = true,
                    Message = "Test recipient added",
                    Data = new AddRecipientData
                    {
                        RecipientCode = Guid.NewGuid().ToString()
                    }
                };
            }

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

        public async Task<BasePaystackResponse<ChargeAuthorizationData>> ChargeAuthorization(ChargeAuthorizationPayload payload, CancellationToken cancellationToken = default)
        {
            if (!_isPaystackEnabled)
            {
                return new BasePaystackResponse<ChargeAuthorizationData>
                {
                    Status = true,
                    Message = "Test authorization charged",
                    Data = new ChargeAuthorizationData
                    {
                        Reference = Guid.NewGuid().ToString(),
                        Status = "success",
                        Amount = payload.Amount,
                        Currency = "NGN",
                        GatewayResponse = "Test gateway response"
                    }
                };
            }

            string uri = "transaction/charge_authorization";

            try
            {
                StringContent content = new(UtilityHelper.Serializer(payload), Encoding.UTF8, "application/json");

                BasePaystackResponse<ChargeAuthorizationData>? response = await _restClientService.PostAsync<BasePaystackResponse<ChargeAuthorizationData>>(uri, content, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<ChargeAuthorizationData>
                    {
                        Status = false,
                        Message = "No data received for authorization charge Paystack",
                        Data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<ChargeAuthorizationData>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BasePaystackResponse<Unit>> DeleteTransferRecipient(string recipientCode, CancellationToken cancellationToken = default)
        {
            if (!_isPaystackEnabled)
            {
                return new BasePaystackResponse<Unit>
                {
                    Status = true,
                    Message = "Test recipient deleted",
                    Data = Unit.Value
                };
            }

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
            if (!_isPaystackEnabled)
            {
                return new BasePaystackResponse<List<BankData>>
                {
                    Status = true,
                    Message = "No test bank",
                    Data = []
                };
            }

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

        public async Task<BasePaystackResponse<InitializeTransactionData>> InitializeTransaction(InitializeTransactionPayload payload, CancellationToken cancellationToken = default)
        {
            if (!_isPaystackEnabled)
            {
                return new BasePaystackResponse<InitializeTransactionData>
                {
                    Status = true,
                    Message = "Test transaction initialized",
                    Data = new InitializeTransactionData
                    {
                        AuthorizationUrl = "https://test.paystack.co/authorize",
                        AccessCode = Guid.NewGuid().ToString(),
                        Reference = Guid.NewGuid().ToString()
                    }
                };
            }

            string uri = "transaction/initialize";

            try
            {
                StringContent content = new(UtilityHelper.Serializer(payload), Encoding.UTF8, "application/json");

                var response = await _restClientService.PostAsync<BasePaystackResponse<InitializeTransactionData>>(uri, content, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<InitializeTransactionData>
                    {
                        Status = false,
                        Message = "No data received for transaction initialization from Paystack",
                        Data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<InitializeTransactionData>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BasePaystackResponse<TransferFundData>> TransferFund(TransferFundPayload payload, CancellationToken cancellationToken = default)
        {
            if (!_isPaystackEnabled)
            {
                return new BasePaystackResponse<TransferFundData>
                {
                    Status = true,
                    Message = "Test fund transfer successful",
                    Data = new TransferFundData
                    {
                        Reference = Guid.NewGuid().ToString(),
                        Status = "success"
                    }
                };
            }

            string uri = "transfer";

            try
            {
                StringContent content = new(UtilityHelper.Serializer(payload), Encoding.UTF8, "application/json");

                var response = await _restClientService.PostAsync<BasePaystackResponse<TransferFundData>>(uri, content, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<TransferFundData>
                    {
                        Status = false,
                        Message = "No data received for fund transfer from Paystack",
                        Data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<TransferFundData>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BasePaystackResponse<VerifyAccountNumberData>> VerifyAccountNumberData(VerifyAccountNumberQuery query, CancellationToken cancellationToken = default)
        {
            if (!_isPaystackEnabled)
            {
                return new BasePaystackResponse<VerifyAccountNumberData>
                {
                    Status = true,
                    Message = "Successfully retrieved test account",
                    Data = new VerifyAccountNumberData
                    {
                        AccountNumber = "0248090000",
                        AccountName = "Test Account Name"
                    }
                };
            }

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

        public async Task<BasePaystackResponse<VerifyTransactionData>> VerifyTransaction(string reference, CancellationToken cancellationToken = default)
        {
            if (!_isPaystackEnabled)
            {
                return new BasePaystackResponse<VerifyTransactionData>
                {
                    Status = true,
                    Message = "Test transaction verified",
                    Data = new VerifyTransactionData
                    {
                        Amount = 10000,
                        Currency = "NGN",
                        TransactionDate = DateTime.UtcNow,
                        Status = "success",
                        Reference = reference,
                        Domain = "test",
                        GatewayResponse = "Test gateway response"
                    }
                };
            }

            string uri = $"transaction/verify/{reference}";

            try
            {
                var response = await _restClientService.GetAsync<BasePaystackResponse<VerifyTransactionData>>(uri, cancellationToken);

                if (response == null)
                {
                    return new BasePaystackResponse<VerifyTransactionData>
                    {
                        Status = false,
                        Message = "No data received for transaction verification from Paystack",
                        Data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new BasePaystackResponse<VerifyTransactionData>
                {
                    Status = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}

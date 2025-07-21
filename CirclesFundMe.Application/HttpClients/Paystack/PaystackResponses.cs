using Serilog;
using System.Net;

namespace CirclesFundMe.Application.HttpClients.Paystack
{
    public record BankData
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("slug")]
        public string? Slug { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }
    }

    public record VerifyAccountNumberData
    {
        [JsonProperty("account_number")]
        public string? AccountNumber { get; set; }

        [JsonProperty("account_name")]
        public string? AccountName { get; set; }
    }

    public record AddRecipientData
    {
        [JsonProperty("recipient_code")]
        public string? RecipientCode { get; set; }
    }

    public record InitializeTransactionData
    {
        [JsonProperty("authorization_url")]
        public string? AuthorizationUrl { get; set; }

        [JsonProperty("access_code")]
        public string? AccessCode { get; set; }

        [JsonProperty("reference")]
        public string? Reference { get; set; }
    }

    public record VerifyTransactionData
    {
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("reference")]
        public string? Reference { get; set; }

        [JsonProperty("domain")]
        public string? Domain { get; set; }

        [JsonProperty("gateway_response")]
        public string? GatewayResponse { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("channel")]
        public string? Channel { get; set; }

        [JsonProperty("ip_address")]
        public string? IpAddress { get; set; }

        [JsonProperty("log")]
        public LogData? Log { get; set; }

        [JsonProperty("fees")]
        public object? Fees { get; set; }

        [JsonProperty("authorization")]
        public AuthorizationData? Authorization { get; set; }

        [JsonProperty("customer")]
        public CustomerData? Customer { get; set; }

        [JsonProperty("plan")]
        public string? Plan { get; set; }
    }

    public class CustomerData
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("customer_code")]
        public string? CustomerCode { get; set; }

        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [JsonProperty("last_name")]
        public string? LastName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
    }

    public record AuthorizationData
    {
        [JsonProperty("authorization_code")]
        public string? AuthorizationCode { get; set; }

        [JsonProperty("card_type")]
        public string? CardType { get; set; }

        [JsonProperty("last4")]
        public string? Last4 { get; set; }

        [JsonProperty("exp_month")]
        public string? ExpMonth { get; set; }

        [JsonProperty("exp_year")]
        public string? ExpYear { get; set; }

        [JsonProperty("bin")]
        public string? Bin { get; set; }

        [JsonProperty("bank")]
        public string? Bank { get; set; }

        [JsonProperty("channel")]
        public string? Channel { get; set; }

        [JsonProperty("reusable")]
        public bool? Reusable { get; set; }

        [JsonProperty("country_code")]
        public string? CountryCode { get; set; }
    }

    public record LogData
    {
        [JsonProperty("time_spent")]
        public int? TimeSpent { get; set; }

        [JsonProperty("attempts")]
        public int? Attempts { get; set; }

        [JsonProperty("authentication")]
        public object? Authentication { get; set; }

        [JsonProperty("errors")]
        public int? Errors { get; set; }

        [JsonProperty("success")]
        public bool? Success { get; set; }

        [JsonProperty("mobile")]
        public bool? Mobile { get; set; }

        [JsonProperty("input")]
        public IList<object>? Input { get; set; }

        [JsonProperty("channel")]
        public object? Channel { get; set; }
    }

    public record TransferFundData
    {
        [JsonProperty("integration")]
        public int? Integration { get; set; }

        [JsonProperty("domain")]
        public string? Domain { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("source")]
        public string? Source { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }

        [JsonProperty("reference")]
        public string? Reference { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("recipient")]
        public string? Recipient { get; set; }

        [JsonProperty("transfer_code")]
        public string? TransferCode { get; set; }
    }

    public record ChargeAuthorizationData
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("reference")]
        public string? Reference { get; set; }

        [JsonProperty("domain")]
        public string? Domain { get; set; }

        [JsonProperty("metadata")]
        public string? Metadata { get; set; }

        [JsonProperty("gateway_response")]
        public string? GatewayResponse { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("channel")]
        public string? Channel { get; set; }

        [JsonProperty("ip_address")]
        public string? IpAddress { get; set; }

        [JsonProperty("log")]
        public string? Log { get; set; }

        [JsonProperty("fees")]
        public int Fees { get; set; }

        [JsonProperty("authorization")]
        public AuthorizationData? Authorization { get; set; }

        [JsonProperty("customer")]
        public CustomerData? Customer { get; set; }

        [JsonProperty("plan")]
        public string? Plan { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    #region Shared
    public record BasePaystackResponse<T>
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("meta")]
        public Meta? Meta { get; set; }
    }

    public record Meta
    {
        [JsonProperty("next")]
        public string? Next { get; set; }

        [JsonProperty("previous")]
        public string? Previous { get; set; }

        [JsonProperty("perPage")]
        public int? PerPage { get; set; }
    }
    #endregion
}

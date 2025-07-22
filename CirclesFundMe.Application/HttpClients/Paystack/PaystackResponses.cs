namespace CirclesFundMe.Application.HttpClients.Paystack
{
    public record BankData
    {
        public string? name { get; set; }
        public string? slug { get; set; }
        public string? code { get; set; }
    }

    public record VerifyAccountNumberData
    {
        public string? account_number { get; set; }
        public string? account_name { get; set; }
    }

    public record AddRecipientData
    {
        public string? recipient_code { get; set; }
    }

    public record InitializeTransactionData
    {
        public string? authorization_url { get; set; }
        public string? access_code { get; set; }
        public string? reference { get; set; }
    }

    public record VerifyTransactionData
    {
        public int? amount { get; set; }
        public string? currency { get; set; }
        public DateTime? transaction_date { get; set; }
        public string? status { get; set; }
        public string? reference { get; set; }
        public string? domain { get; set; }
        public string? gateway_response { get; set; }
        public string? message { get; set; }
        public string? channel { get; set; }
        public string? ip_address { get; set; }
        public AuthorizationData? authorization { get; set; }
        public CustomerData? customer { get; set; }
    }

    public class CustomerData
    {
        public int? id { get; set; }
        public string? customer_code { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? email { get; set; }
    }

    public record AuthorizationData
    {
        public string? authorization_code { get; set; }
        public string? card_type { get; set; }
        public string? last4 { get; set; }
        public string? exp_month { get; set; }
        public string? exp_year { get; set; }
        public string? bin { get; set; }
        public string? bank { get; set; }
        public string? channel { get; set; }
        public bool? reusable { get; set; }
        public string? country_code { get; set; }
    }

    public record TransferFundData
    {
        public int? integration { get; set; }
        public string? domain { get; set; }
        public decimal? Amount { get; set; }
        public string? currency { get; set; }
        public string? source { get; set; }
        public string? reason { get; set; }
        public string? reference { get; set; }
        public string? status { get; set; }
        public string? recipient { get; set; }
        public string? transfer_code { get; set; }
    }

    public record ChargeAuthorizationData
    {
        public decimal amount { get; set; }
        public string? currency { get; set; }
        public DateTime transaction_date { get; set; }
        public string? status { get; set; }
        public string? reference { get; set; }
        public string? domain { get; set; }
        public string? metadata { get; set; }
        public string? gateway_response { get; set; }
        public string? message { get; set; }
        public string? channel { get; set; }
        public string? ip_address { get; set; }
        public AuthorizationData? authorization { get; set; }
    }

    public class PaymentWebhookData
    {
        public string? domain { get; set; }
        public string? status { get; set; }
        public string? reference { get; set; }
        public decimal amount { get; set; }
        public string? message { get; set; }
        public string? gateway_response { get; set; }
        public DateTime paid_at { get; set; }
        public DateTime created_at { get; set; }
        public string? channel { get; set; }
        public string? currency { get; set; }
        public string? ip_address { get; set; }
        public MetaDataObj? metadata { get; set; }
        public AuthorizationData? authorization { get; set; }
    }

    public record MetaDataObj
    {
        public string? userId { get; set; }
    }

    #region Shared
    public record BasePaystackResponse<T>
    {
        public bool status { get; set; }
        public string? message { get; set; }
        public T? data { get; set; }
        public Meta? meta { get; set; }
    }

    public record Meta
    {
        public string? next { get; set; }
        public string? previous { get; set; }
        public int? perPage { get; set; }
    }
    #endregion
}

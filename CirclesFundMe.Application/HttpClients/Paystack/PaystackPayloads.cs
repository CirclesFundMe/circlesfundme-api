﻿namespace CirclesFundMe.Application.HttpClients.Paystack
{
    public record BankDataQuery
    {
        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("use_cursor")]
        public bool UseCursor { get; set; } = true;

        [JsonProperty("perPage")]
        public int PerPage { get; set; } = 100;

        [JsonProperty("next")]
        public string? Next { get; set; }
    }

    public record VerifyAccountNumberQuery
    {
        [JsonProperty("account_number")]
        public string? AccountNumber { get; set; }

        [JsonProperty("bank_code")]
        public string? BankCode { get; set; }
    }

    public record AddTransferRecipientPayload
    {
        [JsonProperty("type")]
        public string? Type { get; set; } = "nuban";

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("account_number")]
        public string? AccountNumber { get; set; }

        [JsonProperty("bank_code")]
        public string? BankCode { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; } = "NGN";
    }

    public record InitializeTransactionPayload
    {
        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; } = "NGN";

        [JsonProperty("reference")]
        public string? Reference { get; set; }

        [JsonProperty("callback_url")]
        public string? CallbackUrl { get; set; }

        [JsonProperty("metadata")]
        public object? Metadata { get; set; }

        [JsonProperty("channels")]
        public List<string>? Channels { get; set; } = ["card"];
    }

    public record TransferFundPayload
    {
        public string? source { get; set; } = "balance";
        public decimal amount { get; set; }
        public string? recipient { get; set; }
        public string? currency { get; set; } = "NGN";
        public string? reference { get; set; }
        public string? reason { get; set; }
        public object? metadata { get; set; }
    }

    public record ChargeAuthorizationPayload
    {
        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("authorization_code")]
        public string? AuthorizationCode { get; set; }
    }
}

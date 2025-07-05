namespace CirclesFundMe.Application.HttpClients.Paystack
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
}

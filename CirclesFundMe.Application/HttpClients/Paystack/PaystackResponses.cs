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
        public int PerPage { get; set; }
    }
    #endregion
}

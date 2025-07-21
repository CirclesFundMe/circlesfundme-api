namespace CirclesFundMe.Application.Models.Finances
{
    public record InitializeTransactionModel
    {
        public string? AuthorizationUrl { get; set; }
        public string? Reference { get; set; }
    }
}

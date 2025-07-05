namespace CirclesFundMe.Application.Models.Finances
{
    public record WalletModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Balance { get; set; }
        public string? Scheme { get; set; }
        public string? Action { get; set; }
        public string? NextTranDate { get; set; }
    }
}

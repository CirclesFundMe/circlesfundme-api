namespace CirclesFundMe.Application.Models.AdminPortal
{
    public record AdminTransactionModel
    {
        public string? Narration { get; set; }
        public decimal Amount { get; set; }
    }
}

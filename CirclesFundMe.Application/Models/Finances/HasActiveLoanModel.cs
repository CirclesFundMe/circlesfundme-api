namespace CirclesFundMe.Application.Models.Finances
{
    public record HasActiveLoanModel
    {
        public bool HasActiveLoan { get; init; }
        public string? Status { get; init; }
    }
}

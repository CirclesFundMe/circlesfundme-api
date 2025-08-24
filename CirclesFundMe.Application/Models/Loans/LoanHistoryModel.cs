namespace CirclesFundMe.Application.Models.Loans
{
    public record LoanHistoryModel
    {
        public decimal AmountRepaid { get; set; }
        public DateTime FirstRepaymentDate { get; set; }
        public DateTime LastRepaymentDate { get; set; }
        public string? Status { get; set; }
        public int RepaymentCount { get; set; }
        public int TotalRepaymentCount { get; set; }
        public int PercentageRepaid => TotalRepaymentCount == 0 ? 0 : (int)((decimal)RepaymentCount / TotalRepaymentCount * 100);
        public string? ApplicationStatus { get; set; }
    }
}

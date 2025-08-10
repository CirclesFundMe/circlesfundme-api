namespace CirclesFundMe.Application.Models.AdminPortal
{
    public record PaymentAdminModel
    {
        public DateTime Date { get; set; }
        public string? Action { get; set; }
        public decimal Amount { get; set; }
        public decimal Charge { get; set; }
        public string? Status { get; set; }
    }
}

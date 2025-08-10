namespace CirclesFundMe.Domain.Entities.Loans
{
    public record LoanApplication : BaseEntity
    {
        public LoanApplicationStatusEnums Status { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal CurrentEligibleAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public SchemeTypeEnums Scheme { get; set; }
        public string? Breakdown { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}

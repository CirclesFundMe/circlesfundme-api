namespace CirclesFundMe.Domain.Pagination.QueryParams.Loans
{
    public record LoanApplicationParams : BaseParam
    {
        public LoanApplicationStatusEnums Status { get; set; }
        public SchemeTypeEnums SchemeType { get; set; }
    }
}

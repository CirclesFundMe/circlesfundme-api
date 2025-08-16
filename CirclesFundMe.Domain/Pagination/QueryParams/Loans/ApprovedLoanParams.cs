namespace CirclesFundMe.Domain.Pagination.QueryParams.Loans
{
    public record ApprovedLoanParams : BaseParam
    {
        public ApprovedLoanStatusEnums Status { get; set; }
    }
}

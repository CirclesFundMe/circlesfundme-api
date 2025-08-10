namespace CirclesFundMe.Domain.Pagination.QueryParams.Finances
{
    public record TransactionParams : BaseParam
    {
        public TransactionTypeEnums TransactionType { get; set; }
    }
}

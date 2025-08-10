namespace CirclesFundMe.Domain.Pagination.QueryParams.Finances
{
    public record PaymentParams : BaseParam
    {
        public PaymentStatusEnums PaymentStatus { get; set; }
    }
}

namespace CirclesFundMe.Application.CQRS.Queries.AdminPortal
{
    public record GetUserPaymentsQuery : IRequest<BaseResponse<PagedList<PaymentAdminModel>>>
    {
        public required PaymentParams Params { get; init; }
        public string? UserId { get; set; }
    }
}

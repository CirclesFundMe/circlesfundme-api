namespace CirclesFundMe.Application.CQRS.Queries.Finances
{
    public record AccountNameEnquiryQuery : IRequest<BaseResponse<string>>
    {
        public required string AccountNumber { get; init; }
        public required string BankCode { get; init; }
    }
}

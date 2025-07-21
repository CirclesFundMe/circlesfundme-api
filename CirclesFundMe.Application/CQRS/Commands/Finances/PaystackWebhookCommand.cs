namespace CirclesFundMe.Application.CQRS.Commands.Finances
{
    public record PaystackWebhookCommand : IRequest<BaseResponse<bool>>
    {
        public string? Event { get; set; }
        public object? Data { get; set; }
    }
}

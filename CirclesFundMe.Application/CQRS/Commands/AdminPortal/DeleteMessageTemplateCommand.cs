namespace CirclesFundMe.Application.CQRS.Commands.AdminPortal
{
    public record DeleteMessageTemplateCommand : IRequest<BaseResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}

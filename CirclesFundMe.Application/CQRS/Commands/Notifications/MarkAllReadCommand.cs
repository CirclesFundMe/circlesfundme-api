namespace CirclesFundMe.Application.CQRS.Commands.Notifications
{
    public record MarkAllReadCommand : IRequest<BaseResponse<bool>>
    {
    }
}

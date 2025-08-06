using CirclesFundMe.Application.Models.AdminPortal;

namespace CirclesFundMe.Application.CQRS.Queries.AdminPortal
{
    public record GetMessageTemplatesQuery : IRequest<BaseResponse<List<MessageTemplateModel>>>
    {
    }
}

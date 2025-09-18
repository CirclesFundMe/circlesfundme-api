namespace CirclesFundMe.Application.CQRS.Commands.Utility
{
    public record UploadDocumentCommand(IFormFile Document) : IRequest<BaseResponse<string>>;
}

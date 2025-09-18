namespace CirclesFundMe.Application.CQRS.CommandHandlers.Utility
{
    public class UploadDocumentCommandHandler(ICloudinaryDocumentService cloudinaryDocumentService) : IRequestHandler<UploadDocumentCommand, BaseResponse<string>>
    {
        private readonly ICloudinaryDocumentService _cloudinaryDocumentService = cloudinaryDocumentService;

        public async Task<BaseResponse<string>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            if (request.Document == null || request.Document.Length == 0)
            {
                return BaseResponse<string>.BadRequest("Document is empty.");
            }

            string url = await _cloudinaryDocumentService.UploadDocumentAsync(request.Document);
            if (string.IsNullOrWhiteSpace(url))
            {
                return BaseResponse<string>.BadRequest("Failed to upload document.");
            }
            return BaseResponse<string>.Success(url, "Document uploaded successfully.");
        }
    }
}

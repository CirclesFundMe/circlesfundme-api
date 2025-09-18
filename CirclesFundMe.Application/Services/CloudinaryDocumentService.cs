namespace CirclesFundMe.Application.Services
{
    public interface ICloudinaryDocumentService
    {
        Task<string> UploadDocumentAsync(IFormFile file, string? customFileName = null);
    }

    public class CloudinaryDocumentService(Cloudinary cloudinary, ILogger<CloudinaryDocumentService> logger) : ICloudinaryDocumentService
    {
        private readonly Cloudinary _cloudinary = cloudinary;
        private readonly ILogger<CloudinaryDocumentService> _logger = logger;

        public async Task<string> UploadDocumentAsync(IFormFile file, string? customFileName = null)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogError("Document is empty.");
                return string.Empty;
            }

            using var stream = file.OpenReadStream();

            // Clean up the file name as a slug
            string baseName = !string.IsNullOrWhiteSpace(customFileName)
                ? Path.GetFileNameWithoutExtension(customFileName)
                : Path.GetFileNameWithoutExtension(file.FileName);

            string publicId = ToSlug(baseName);

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId
            };

            RawUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                return uploadResult.SecureUrl?.AbsoluteUri ?? string.Empty;

            _logger.LogError("Document upload failed: {Error}", uploadResult.Error?.Message);
            return string.Empty;
        }

        // Utility method to slugify file names
        private static string ToSlug(string value)
        {
            value = value.ToLowerInvariant();
            value = Regex.Replace(value, @"\s+", "-"); // Replace spaces with hyphens
            value = Regex.Replace(value, @"[^a-z0-9\-]", ""); // Remove invalid chars
            value = Regex.Replace(value, @"-+", "-"); // Replace multiple hyphens
            value = value.Trim('-');
            return value;
        }
    }
}

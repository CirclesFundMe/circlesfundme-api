using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace CirclesFundMe.Application.Services
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile file);
        Task DeleteFileAsync(string fileUrl);
    }
    public class ImageService(Cloudinary cloudinary, ILogger<ImageService> logger) : IImageService
    {
        private readonly Cloudinary _cloudinary = cloudinary;
        private readonly ILogger<ImageService> _logger = logger;

        public async Task<string> UploadImage(IFormFile file)
        {
            ImageUploadResult uploadResult;

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Quality("auto")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult.SecureUrl.AbsoluteUri;
            }
            else
            {
                _logger.LogError("Image upload failed");
                return string.Empty;
            }
        }
        public async Task DeleteFileAsync(string fileUrl)
        {
            Uri uri = new(fileUrl);
            string publicId = uri.Segments.Last().Split('.')[0];

            DeletionParams deletionParams = new(publicId);
            DeletionResult deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            _logger.LogInformation($"Cloudinary Image Deletion result: {UtilityHelper.Serializer(deletionResult)}\n");
        }
    }
}

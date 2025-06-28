namespace CirclesFundMe.Application.Services
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile file);
        Task<string> UploadImageFromBase64(string base64Image, string fileName);
        Task DeleteFileAsync(string fileUrl);
        Task<string> GetImageAsBase64Async(string imageUrl);
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

        public async Task<string> UploadImageFromBase64(string base64Image, string fileName)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
            {
                _logger.LogError("Base64 image string is null or empty");
                return string.Empty;
            }

            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64Image);
                using MemoryStream stream = new(imageBytes);

                ImageUploadParams uploadParams = new()
                {
                    File = new FileDescription(fileName, stream),
                    Transformation = new Transformation().Quality("auto")
                };

                ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.AbsoluteUri;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload image from base64 string");
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

        public async Task<string> GetImageAsBase64Async(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return string.Empty;

            try
            {
                using var httpClient = new HttpClient();
                var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                return Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve or convert image from URL.");
                return string.Empty;
            }
        }

    }
}

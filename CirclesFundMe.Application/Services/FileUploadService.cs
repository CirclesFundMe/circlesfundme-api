namespace CirclesFundMe.Application.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadAsync(IFormFile file);
        Task DeleteAsync(string fileUrl);
    }

    public class FileUploadService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : IFileUploadService
    {
        private readonly IWebHostEnvironment _env = env;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<string> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            string ext = Path.GetExtension(file.FileName);
            string fileName = $"{Guid.NewGuid():N}{ext}";
            string folderPath = Path.Combine(_env.WebRootPath, "files");
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            HttpRequest request = (_httpContextAccessor?.HttpContext?.Request) ?? throw new InvalidOperationException("HTTP context is not available.");

            string baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/files/{fileName}";
        }

        public async Task DeleteAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                throw new ArgumentException("File URL is required.");

            string fileName = Path.GetFileName(fileUrl);
            string filePath = Path.Combine(_env.WebRootPath, "files", fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await Task.CompletedTask;
        }
    }
}

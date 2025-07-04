namespace CirclesFundMe.Application.Services
{
    public interface IRestClientService
    {
        Task<T?> GetAsync<T>(string uri, CancellationToken cancellationToken = default);
        Task<TResult?> PostAsync<TResult>(string uri, HttpContent data, CancellationToken cancellationToken = default);
        Task<TResult?> PutAsync<TRequest, TResult>(string uri, TRequest data, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string uri, CancellationToken cancellationToken = default);
    }

    public class RestClientService : IRestClientService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger _logger;

        public RestClientService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T?> GetAsync<T>(string uri, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Sending GET request to {Uri}", uri);

                HttpResponseMessage response = await _httpClient.GetAsync(uri, cancellationToken);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>(_serializerOptions, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while fetching data from {Uri}", uri);
                throw new Exception($"Error fetching data from {uri}: {ex.Message}", ex);
            }
        }

        public async Task<TResult?> PostAsync<TResult>(string uri, HttpContent data, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(uri, data, cancellationToken);
            return await response.Content.ReadFromJsonAsync<TResult>(_serializerOptions, cancellationToken);
        }

        public async Task<TResult?> PutAsync<TRequest, TResult>(string uri, TRequest data, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(uri, data, _serializerOptions, cancellationToken);
            return await response.Content.ReadFromJsonAsync<TResult>(_serializerOptions, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string uri, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(uri, cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}

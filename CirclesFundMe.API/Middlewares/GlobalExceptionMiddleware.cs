namespace CirclesFundMe.API.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            _logger.LogError($"Error Processing Request...\nException Message: {ex.Message}\nTrace: {ex.StackTrace}\n");

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string errorMessage = ex.InnerException?.Message ?? ex.Message;

            BaseResponse<object> res = new()
            {
                IsSuccess = false,
                StatusCode = ResponseCodes.InternalServer,
                Message = ResponseMessages.InternalServer,
                Data = errorMessage
            };

            JsonSerializerSettings options = new() { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(res, options));
        }
    }
}

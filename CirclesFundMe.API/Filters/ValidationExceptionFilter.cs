namespace CirclesFundMe.API.Filters
{
    public class ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger) : ActionFilterAttribute
    {
        private readonly ILogger<ValidationExceptionFilter> _logger = logger;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errors = context.ModelState.Where(ms => ms.Value?.Errors.Count > 0)
                    .SelectMany(ms => ms.Value?.Errors.Select(e => new { Field = ms.Key, e.ErrorMessage }) ?? [])
                    .ToList();

                _logger.LogError($"Validation error(s) occurred :: {UtilityHelper.Serializer(errors)}");

                string topError = errors.FirstOrDefault()?.ErrorMessage ?? ResponseMessages.ValidationError;

                BaseResponse<object> result = new()
                {
                    IsSuccess = false,
                    StatusCode = ResponseCodes.BadRequest,
                    Message = topError,
                    Data = errors
                };

                context.Result = new JsonResult(result);
            }
        }
    }
}

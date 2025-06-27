namespace CirclesFundMe.Application.Contants
{
    public record BaseResponse<T>
    {
        public bool IsSuccess { get; set; } = true;
        public string StatusCode { get; set; } = ResponseCodes.Ok;
        public string Message { get; set; } = ResponseMessages.Ok;
        public T? Data { get; set; }
        public object? MetaData { get; set; }

        public static BaseResponse<T> Success(T data, string message = ResponseMessages.Ok, object? metaData = null)
        {
            return new BaseResponse<T>
            {
                StatusCode = ResponseCodes.Ok,
                IsSuccess = true,
                Message = message,
                Data = data,
                MetaData = metaData
            };
        }

        public static BaseResponse<T> NotFound(string message, object? metaData = null)
        {
            return new BaseResponse<T>
            {
                StatusCode = ResponseCodes.NotFound,
                IsSuccess = false,
                Message = message,
                Data = default,
                MetaData = metaData
            };
        }

        public static BaseResponse<T> BadRequest(string message, object? metaData = null)
        {
            return new BaseResponse<T>
            {
                StatusCode = ResponseCodes.BadRequest,
                IsSuccess = false,
                Message = message,
                Data = default,
                MetaData = metaData
            };
        }

        public static BaseResponse<T> Unauthorized(string message, object? metaData = null)
        {
            return new BaseResponse<T>
            {
                StatusCode = ResponseCodes.Unauthorized,
                IsSuccess = false,
                Message = message,
                Data = default,
                MetaData = metaData
            };
        }

        public static BaseResponse<T> Forbidden(string message, object? metaData = null)
        {
            return new BaseResponse<T>
            {
                StatusCode = ResponseCodes.Forbidden,
                IsSuccess = false,
                Message = message,
                Data = default,
                MetaData = metaData
            };
        }

        public static BaseResponse<T> Conflict(string message, object? metaData = null)
        {
            return new BaseResponse<T>
            {
                StatusCode = ResponseCodes.Conflict,
                IsSuccess = false,
                Message = message,
                Data = default,
                MetaData = metaData
            };
        }
    }
}

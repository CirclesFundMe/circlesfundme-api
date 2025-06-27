namespace CirclesFundMe.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public abstract class BaseControllerV1 : ControllerBase
    {
        protected IActionResult HandleResponse<T>(BaseResponse<T> response)
        {
            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Response is null." });
            }

            return response.StatusCode switch
            {
                ResponseCodes.Ok => Ok(response),
                ResponseCodes.NotFound => NotFound(response),
                ResponseCodes.BadRequest => BadRequest(response),
                ResponseCodes.Unauthorized => Unauthorized(response),
                ResponseCodes.Forbidden => Forbid(),
                ResponseCodes.Conflict => Conflict(response),
                ResponseCodes.InternalServer => StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }
    }
}

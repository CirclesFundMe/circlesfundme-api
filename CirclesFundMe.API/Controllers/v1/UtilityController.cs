namespace CirclesFundMe.API.Controllers.v1
{
    public class UtilityController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpPost("contact-us")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Send Contact Us Mail")]
        public async Task<IActionResult> SendContactUsMail([FromBody] SendContactUsMailCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellationToken);
            return HandleResponse(response);
        }
    }
}

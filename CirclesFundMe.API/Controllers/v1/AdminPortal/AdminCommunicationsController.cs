namespace CirclesFundMe.API.Controllers.v1.AdminPortal
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public class AdminCommunicationsController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpGet("all")]
        [ProducesResponseType<BaseResponse<PagedList<CommunicationModel>>>(200)]
        [SwaggerOperation(Summary = "Get Communications for Admin Portal")]
        public async Task<IActionResult> GetCommunications([FromQuery] CommunicationParams @params, CancellationToken cancellation)
        {
            BaseResponse<PagedList<CommunicationModel>> response = await _sender.Send(new GetCommunicationsQuery { Params = @params }, cancellation);
            return HandleResponse(response);
        }

        [HttpPost("send")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Create Communication")]
        public async Task<IActionResult> CreateCommunication([FromBody] CreateCommunicationCommand command, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }
    }
}

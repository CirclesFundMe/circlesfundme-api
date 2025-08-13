namespace CirclesFundMe.API.Controllers.v1.AdminPortal
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public class AdminMessageTemplatesController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpGet("message-templates")]
        [ProducesResponseType<BaseResponse<List<MessageTemplateModel>>>(200)]
        [SwaggerOperation(Summary = "Get Message Templates for Admin Portal")]
        public async Task<IActionResult> GetMessageTemplates(CancellationToken cancellation)
        {
            BaseResponse<List<MessageTemplateModel>> response = await _sender.Send(new GetMessageTemplatesQuery(), cancellation);
            return HandleResponse(response);
        }

        [HttpDelete("message-templates/{id}")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Delete Message Template")]
        public async Task<IActionResult> DeleteMessageTemplate(Guid id, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(new DeleteMessageTemplateCommand { Id = id }, cancellation);
            return HandleResponse(response);
        }

        [HttpPut("message-templates/{id}")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Update Message Template")]
        public async Task<IActionResult> UpdateMessageTemplate(Guid id, [FromBody] UpdateMessageTemplateCommand command, CancellationToken cancellation)
        {
            command.Id = id;
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }

        [HttpPost("message-templates")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Create Message Template")]
        public async Task<IActionResult> CreateMessageTemplate([FromBody] CreateMessageTemplateCommand command, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }
    }
}

namespace CirclesFundMe.API.Controllers.v1.AdminPortal
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public class AdminUserManagementController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpGet("users")]
        [ProducesResponseType<BaseResponse<PagedList<AdminUserModel>>>(200)]
        [SwaggerOperation(Summary = "Get Users for Admin Portal")]
        public async Task<IActionResult> GetUsers([FromQuery] AdminUserParams queryParams, CancellationToken cancellation)
        {
            BaseResponse<PagedList<AdminUserModel>> response = await _sender.Send(new GetAdminUsersQuery { Params = queryParams }, cancellation);
            return HandleResponse(response);
        }

        [HttpPut("users/{userId}/deactivate")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Deactivate User")]
        public async Task<IActionResult> DeactivateUser(string userId, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(new DeactivateUserCommand { UserId = userId }, cancellation);
            return HandleResponse(response);
        }

        [HttpPut("users/{userId}/reactivate")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Reactivate User")]
        public async Task<IActionResult> ReactivateUser(string userId, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(new ReactivateUserCommand { UserId = userId }, cancellation);
            return HandleResponse(response);
        }

        [HttpGet("users/{userId}/payments")]
        [ProducesResponseType<BaseResponse<PagedList<PaymentAdminModel>>>(200)]
        [SwaggerOperation(Summary = "Get User Payments")]
        public async Task<IActionResult> GetUserPayments(string userId, [FromQuery] PaymentParams queryParams, CancellationToken cancellation)
        {
            BaseResponse<PagedList<PaymentAdminModel>> response = await _sender.Send(new GetUserPaymentsQuery { UserId = userId, Params = queryParams }, cancellation);
            return HandleResponse(response);
        }
    }
}

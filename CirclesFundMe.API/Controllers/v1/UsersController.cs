namespace CirclesFundMe.API.Controllers.v1
{
    [Authorize]
    public class UsersController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpGet]
        [ProducesResponseType<BaseResponse<PagedList<UserModel>>>(200)]
        [SwaggerOperation(Summary = "Get all Users")]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams @params, CancellationToken cancellation)
        {
            BaseResponse<PagedList<UserModel>> response = await _sender.Send(new GetUsersQuery { Params = @params }, cancellation);
            return HandleResponse(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType<BaseResponse<UserModel>>(200)]
        [SwaggerOperation(Summary = "Get a User by ID")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Member}")]
        public async Task<IActionResult> GetUser(string id, CancellationToken cancellation)
        {
            BaseResponse<UserModel> response = await _sender.Send(new GetUserByIdQuery { UserId = id }, cancellation);
            return HandleResponse(response);
        }

        [HttpGet("me")]
        [ProducesResponseType<BaseResponse<UserModel>>(200)]
        [SwaggerOperation(Summary = "Get my profile")]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellation)
        {
            BaseResponse<UserModel> response = await _sender.Send(new GetMyProfileQuery(), cancellation);
            return HandleResponse(response);
        }

        [HttpPut("update")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Update User")]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand command, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }

        [HttpPost("change-password")]
        [ProducesResponseType<BaseResponse<string>>(200)]
        [SwaggerOperation(Summary = "Change Password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken cancellation)
        {
            BaseResponse<string> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }

        [HttpPost("change-profile-picture")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Change Profile Picture")]
        public async Task<IActionResult> ChangeProfilePicture([FromForm] ChangeProfilePictureCommand command, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }

        [HttpPost("create-withdrawal-setting")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Create Withdrawal Setting")]
        public async Task<IActionResult> CreateWithdrawalSetting(CreateWithdrawalSettingCommand command, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }

        [HttpPut("update-withdrawal-setting")]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        [SwaggerOperation(Summary = "Update Withdrawal Setting")]
        public async Task<IActionResult> UpdateWithdrawalSetting(UpdateWithdrawalSettingCommand command, CancellationToken cancellation)
        {
            BaseResponse<bool> response = await _sender.Send(command, cancellation);
            return HandleResponse(response);
        }
    }
}

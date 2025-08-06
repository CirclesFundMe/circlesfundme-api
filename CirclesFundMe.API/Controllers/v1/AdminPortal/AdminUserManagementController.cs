using CirclesFundMe.Application.CQRS.Queries.AdminPortal;
using CirclesFundMe.Application.Models.AdminPortal;
using CirclesFundMe.Domain.Pagination.QueryParams.AdminPortal;

namespace CirclesFundMe.API.Controllers.v1.AdminPortal
{
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
    }
}

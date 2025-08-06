namespace CirclesFundMe.API.Controllers.v1.AdminPortal
{
    public class AdminUserManagementController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;
    }
}

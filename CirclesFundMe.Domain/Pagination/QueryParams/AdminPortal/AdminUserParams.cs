using CirclesFundMe.Domain.Enums.AdminPortal;

namespace CirclesFundMe.Domain.Pagination.QueryParams.AdminPortal
{
    public record AdminUserParams : BaseParam
    {
        public AdminUserStatus Status { get; set; }
    }
}

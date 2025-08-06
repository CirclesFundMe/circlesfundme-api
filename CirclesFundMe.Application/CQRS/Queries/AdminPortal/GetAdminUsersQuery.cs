using CirclesFundMe.Application.Models.AdminPortal;
using CirclesFundMe.Domain.Pagination.QueryParams.AdminPortal;

namespace CirclesFundMe.Application.CQRS.Queries.AdminPortal
{
    public record GetAdminUsersQuery : IRequest<BaseResponse<PagedList<AdminUserModel>>>
    {
        public required AdminUserParams Params { get; init; }
    }
}

using CirclesFundMe.Domain.Pagination.QueryParams.AdminPortal;

namespace CirclesFundMe.Domain.RepositoryContracts.AdminPortal
{
    public interface IUserManagementRepository
    {
        Task<PagedList<AppUserAdmin>> GetUsersAsync(AdminUserParams @params, CancellationToken cancellation);
    }
}

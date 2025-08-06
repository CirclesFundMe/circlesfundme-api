using CirclesFundMe.Domain.Pagination.QueryParams.AdminPortal;

namespace CirclesFundMe.Domain.RepositoryContracts.AdminPortal
{
    public interface IUserManagementRepository
    {
        Task<PagedList<AppUserAdmin>> GetUsersAsync(AdminUserParams @params, CancellationToken cancellation);
        Task<bool> DeactivateUser(string userId, CancellationToken cancellation);
        Task<bool> ReactivateUser(string userId, CancellationToken cancellation);
    }
}

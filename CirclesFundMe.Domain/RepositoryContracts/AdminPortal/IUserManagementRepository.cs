namespace CirclesFundMe.Domain.RepositoryContracts.AdminPortal
{
    public interface IUserManagementRepository
    {
        Task<PagedList<AppUserAdmin>> GetUsersAsync(AdminUserParams @params, CancellationToken cancellation);
        Task<bool> DoesHavPendingKYC(string userId, CancellationToken cancellation);
        Task<bool> DeactivateUser(string userId, CancellationToken cancellation);
        Task<bool> ReactivateUser(string userId, CancellationToken cancellation);
        Task<IEnumerable<AppUser>> GetUsersByCommunicationTarget(CommunicationTarget target);
    }
}

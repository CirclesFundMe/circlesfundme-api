namespace CirclesFundMe.Domain.RepositoryContracts.Users
{
    public interface IUserRepository
    {
        Task<PagedList<AppUser>> GetUsersAsync(UserParams userParams, CancellationToken cancellation);
        Task<AppUserExtension?> GetUserByIdAsync(string id, CancellationToken cancellation, bool isAdmin = false);
        Task<AppUser?> GetUserByIdMiniAsync(string id, CancellationToken cancellation);
    }
}

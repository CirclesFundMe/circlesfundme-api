namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserKYCRepository(DbSet<UserKYC> userKYCs) : RepositoryBase<UserKYC>(userKYCs), IUserKYCRepository
    {
    }
}

namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserKYCRepository(SqlDbContext context) : RepositoryBase<UserKYC>(context.UserKYCs), IUserKYCRepository
    {
    }
}

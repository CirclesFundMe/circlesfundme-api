namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserOtpRepository(SqlDbContext context) : RepositoryBase<UserOtp>(context.UserOtps), IUserOtpRepository
    {
    }
}

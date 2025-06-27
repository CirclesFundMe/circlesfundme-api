namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserOtpRepository(DbSet<UserOtp> userOtps) : RepositoryBase<UserOtp>(userOtps), IUserOtpRepository
    {
    }
}

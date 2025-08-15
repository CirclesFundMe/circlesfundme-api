namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserWithdrawalSettingRepository(SqlDbContext context) : RepositoryBase<UserWithdrawalSetting>(context.UserWithdrawalSettings), IUserWithdrawalSettingRepository
    {
    }
}

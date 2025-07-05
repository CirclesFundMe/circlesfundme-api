namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserWithdrawalSettingRepository(DbSet<UserWithdrawalSetting> userWithdrawalSettings) : RepositoryBase<UserWithdrawalSetting>(userWithdrawalSettings), IUserWithdrawalSettingRepository
    {
    }
}

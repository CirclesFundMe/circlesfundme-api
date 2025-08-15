namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserAddressRepository(SqlDbContext context) : RepositoryBase<UserAddress>(context.UserAddresses), IUserAddressRepository
    {
    }
}

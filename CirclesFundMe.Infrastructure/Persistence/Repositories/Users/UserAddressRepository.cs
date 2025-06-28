namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserAddressRepository(DbSet<UserAddress> userAddresses) : RepositoryBase<UserAddress>(userAddresses), IUserAddressRepository
    {
    }
}

namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserContributionRepository(DbSet<UserContribution> contributions) : RepositoryBase<UserContribution>(contributions), IUserContributionRepository
    {
    }
}

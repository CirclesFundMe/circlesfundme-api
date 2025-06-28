namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserContributionSchemeRepository(DbSet<UserContributionScheme> userContributionSchemes) : RepositoryBase<UserContributionScheme>(userContributionSchemes), IUserContributionSchemeRepository
    {
    }
}

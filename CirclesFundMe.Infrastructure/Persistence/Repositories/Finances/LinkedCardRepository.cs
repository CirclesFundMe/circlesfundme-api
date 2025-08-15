namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Finances
{
    public class LinkedCardRepository(SqlDbContext context) : RepositoryBase<LinkedCard>(context.LinkedCards), ILinkedCardRepository
    {
    }
}

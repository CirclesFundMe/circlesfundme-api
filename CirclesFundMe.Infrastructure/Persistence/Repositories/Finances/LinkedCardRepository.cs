namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Finances
{
    public class LinkedCardRepository(DbSet<LinkedCard> linkedCards) : RepositoryBase<LinkedCard>(linkedCards), ILinkedCardRepository
    {
    }
}

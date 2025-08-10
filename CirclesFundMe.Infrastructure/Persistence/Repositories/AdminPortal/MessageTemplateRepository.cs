namespace CirclesFundMe.Infrastructure.Persistence.Repositories.AdminPortal
{
    public class MessageTemplateRepository(DbSet<MessageTemplate> templates) : RepositoryBase<MessageTemplate>(templates), IMessageTemplateRepository
    {
    }
}

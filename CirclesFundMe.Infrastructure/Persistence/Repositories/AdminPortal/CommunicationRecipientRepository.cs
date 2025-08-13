namespace CirclesFundMe.Infrastructure.Persistence.Repositories.AdminPortal
{
    public class CommunicationRecipientRepository(SqlDbContext context) : RepositoryBase<CommunicationRecipient>(context.CommunicationRecipients), ICommunicationRecipientRepository
    {
    }
}

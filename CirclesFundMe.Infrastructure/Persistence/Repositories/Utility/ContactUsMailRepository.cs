namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Utility
{
    public class ContactUsMailRepository(DbSet<ContactUsMail> contactUsMail) : RepositoryBase<ContactUsMail>(contactUsMail), IContactUsMailRepository
    {
    }
}

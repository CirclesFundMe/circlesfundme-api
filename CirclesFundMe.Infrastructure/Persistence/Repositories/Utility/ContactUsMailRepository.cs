namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Utility
{
    public class ContactUsMailRepository(SqlDbContext context) : RepositoryBase<ContactUsMail>(context.ContactUsMails), IContactUsMailRepository
    {
    }
}

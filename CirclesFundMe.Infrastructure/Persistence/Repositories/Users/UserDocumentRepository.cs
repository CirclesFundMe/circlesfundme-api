namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserDocumentRepository(DbSet<UserDocument> userDocuments) : RepositoryBase<UserDocument>(userDocuments), IUserDocumentRepository
    {
    }
}

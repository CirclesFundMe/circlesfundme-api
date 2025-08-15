namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserDocumentRepository(SqlDbContext context) : RepositoryBase<UserDocument>(context.UserDocuments), IUserDocumentRepository
    {
    }
}

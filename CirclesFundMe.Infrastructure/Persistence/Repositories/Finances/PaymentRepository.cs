namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Finances
{
    public class PaymentRepository(DbSet<Payment> payments) : RepositoryBase<Payment>(payments), IPaymentRepository
    {
    }
}

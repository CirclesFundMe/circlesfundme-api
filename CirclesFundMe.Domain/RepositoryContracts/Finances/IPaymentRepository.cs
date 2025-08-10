namespace CirclesFundMe.Domain.RepositoryContracts.Finances
{
    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
        Task<PagedList<PaymentAdmin>> GetUserPaymentsForAdmin(string userId, PaymentParams @params, CancellationToken cancellation);
    }
}

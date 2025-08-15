namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Finances
{
    public class PaymentRepository(SqlDbContext context) : RepositoryBase<Payment>(context.Payments), IPaymentRepository
    {
        private readonly DbSet<Payment> _payments = context.Payments;
        public async Task<PagedList<PaymentAdmin>> GetUserPaymentsForAdmin(string userId, PaymentParams @params, CancellationToken cancellation)
        {
            IQueryable<Payment> query = _payments
                .AsNoTracking()
                .Where(p => p.UserId == userId);

            if (Enum.IsDefined(@params.PaymentStatus))
            {
                query = query.Where(p => p.PaymentStatus == @params.PaymentStatus);
            }

            int totalCount = await query.CountAsync(cancellation);

            var items = await query
                .OrderByDescending(p => p.CreatedDate)
                .Skip((@params.PageNumber - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .Select(p => new PaymentAdmin
                {
                    Date = p.CreatedDate,
                    Action = p.PaymentType == PaymentTypeEnums.Inflow ? "Contribution" : "Withdrawal",
                    Amount = p.Amount,
                    Charge = p.ChargeAmount,
                    Status = p.PaymentStatus
                })
                .ToListAsync(cancellation);

            return PagedList<PaymentAdmin>.ToPagedList(items, totalCount, @params.PageNumber, @params.PageSize);
        }
    }
}

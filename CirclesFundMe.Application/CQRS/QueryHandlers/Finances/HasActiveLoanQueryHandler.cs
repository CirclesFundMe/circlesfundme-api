namespace CirclesFundMe.Application.CQRS.QueryHandlers.Finances
{
    public class HasActiveLoanQueryHandler(SqlDbContext context, ICurrentUserService currentUserService) : IRequestHandler<HasActiveLoanQuery, BaseResponse<HasActiveLoanModel>>
    {
        private readonly SqlDbContext _context = context;
        private readonly string _userId = currentUserService.UserId;

        public async Task<BaseResponse<HasActiveLoanModel>> Handle(HasActiveLoanQuery request, CancellationToken cancellationToken)
        {
            string? hasActiveLoan = await _context.LoanApplications
                .AsNoTracking()
                .Where(x => x.UserId == _userId &&
                    (x.Status == LoanApplicationStatusEnums.Pending || x.Status == LoanApplicationStatusEnums.Waitlist))
                .Select(x => x.Status.ToString())
                .Union(
                    _context.ApprovedLoans
                        .AsNoTracking()
                        .Where(x => x.UserId == _userId && x.Status == ApprovedLoanStatusEnums.Active)
                        .Select(x => x.Status.ToString())
                )
                .FirstOrDefaultAsync(cancellationToken);

            var model = new HasActiveLoanModel
            {
                HasActiveLoan = hasActiveLoan != null,
                Status = hasActiveLoan
            };

            return BaseResponse<HasActiveLoanModel>.Success(model,
                hasActiveLoan != null ? "User has an active loan." : "User does not have an active loan.");
        }
    }
}

namespace CirclesFundMe.Application.CQRS.QueryHandlers.Finances
{
    public class HasActiveLoanQueryHandler(SqlDbContext context, ICurrentUserService currentUserService) : IRequestHandler<HasActiveLoanQuery, BaseResponse<bool>>
    {
        private readonly SqlDbContext _context = context;
        private readonly string _userId = currentUserService.UserId;

        public async Task<BaseResponse<bool>> Handle(HasActiveLoanQuery request, CancellationToken cancellationToken)
        {
            bool hasActiveLoan = await _context.LoanApplications
                .AsNoTracking()
                .Where(x => x.UserId == _userId &&
                    (x.Status == LoanApplicationStatusEnums.Pending || x.Status == LoanApplicationStatusEnums.Waitlist))
                .Select(x => 1)
                .Union(
                    _context.ApprovedLoans
                        .AsNoTracking()
                        .Where(x => x.UserId == _userId && x.Status == ApprovedLoanStatusEnums.Active)
                        .Select(x => 1)
                )
                .AnyAsync(cancellationToken);

            return hasActiveLoan
                ? BaseResponse<bool>.Success(true, "User has an active loan.")
                : BaseResponse<bool>.Success(false, "User does not have an active loan.");
        }
    }
}

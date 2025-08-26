namespace CirclesFundMe.Application.CQRS.QueryHandlers.Finances
{
    public class HasActiveLoanQueryHandler(SqlDbContext context, ICurrentUserService currentUserService) : IRequestHandler<HasActiveLoanQuery, BaseResponse<HasActiveLoanModel>>
    {
        private readonly SqlDbContext _context = context;
        private readonly string _userId = currentUserService.UserId;

        public async Task<BaseResponse<HasActiveLoanModel>> Handle(HasActiveLoanQuery request, CancellationToken cancellationToken)
        {
            string sql = @"
            SELECT TOP 1 CAST(Status AS NVARCHAR(50)) AS Status
            FROM [CFM].[LoanApplications]
            WHERE UserId = {0} AND (Status = {1} OR Status = {2})
            UNION
            SELECT TOP 1 CAST(Status AS NVARCHAR(50)) AS Status
            FROM [CFM].[ApprovedLoans]
            WHERE UserId = {0} AND Status = {3}";

            try
            {
                StatusResult? result = await _context.Database
                    .SqlQueryRaw<StatusResult>(
                        sql,
                        _userId,
                        LoanApplicationStatusEnums.Pending.ToString(),
                        LoanApplicationStatusEnums.Waitlist.ToString(),
                        ApprovedLoanStatusEnums.Active.ToString()
                    )
                    .FirstOrDefaultAsync(cancellationToken);

                HasActiveLoanModel model = new()
                {
                    HasActiveLoan = result != null,
                    Status = result?.Status
                };

                return BaseResponse<HasActiveLoanModel>.Success(model,
                    result != null ? "User has an active loan." : "User does not have an active loan.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BaseResponse<HasActiveLoanModel>.BadRequest("An error occurred while checking for active loans.");
            }
        }

        // Helper class for mapping the SQL result
        private class StatusResult
        {
            public string? Status { get; set; }
        }
    }
}

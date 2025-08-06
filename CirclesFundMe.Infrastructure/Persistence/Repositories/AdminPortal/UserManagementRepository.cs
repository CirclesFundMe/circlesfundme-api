using CirclesFundMe.Domain.Enums.AdminPortal;
using CirclesFundMe.Domain.Pagination.QueryParams.AdminPortal;
using CirclesFundMe.Domain.RepositoryContracts.AdminPortal;

namespace CirclesFundMe.Infrastructure.Persistence.Repositories.AdminPortal
{
    public class UserManagementRepository(SqlDbContext context) : IUserManagementRepository
    {
        private readonly SqlDbContext _context = context;

        public async Task<PagedList<AppUserAdmin>> GetUsersAsync(AdminUserParams @params, CancellationToken cancellation)
        {
            IQueryable<AppUser> query = _context.Users
                .AsNoTracking().Where(u => u.UserContributionScheme != null);

            if (!string.IsNullOrEmpty(@params.SearchKey))
            {
                query = query.Where(u => u.FirstName.Contains(@params.SearchKey) || u.LastName.Contains(@params.SearchKey));
            }

            switch (@params.Status)
            {
                case AdminUserStatus.Active:
                    query = query.Where(u => !u.IsDeleted);
                    break;
                case AdminUserStatus.Deactivated:
                    query = query.Where(u => u.IsDeleted);
                    break;
                case AdminUserStatus.PendingKYC:
                    query = query.Where(u => u.UserKYC == null || (u.UserKYC != null && (u.UserKYC.BVN == null)));
                    break;
                default:
                    break;
            }

            int totalCount = await query.CountAsync(cancellation);

            var items = await query
                .OrderByDescending(u => u.CreatedDate)
                .Skip(@params.PageSize * (@params.PageNumber - 1))
                .Take(@params.PageSize)
                .Select(u => new AppUserAdmin
                {
                    UserId = u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    DateJoined = u.CreatedDate,
                    SchemeType = u.UserContributionScheme != null ? u.UserContributionScheme.ContributionScheme!.SchemeType : SchemeTypeEnums.Weekly,
                    Scheme = u.UserContributionScheme != null ?
                    u.UserContributionScheme.ContributionScheme != null ? u.UserContributionScheme.ContributionScheme.Name : "N/A"
                    : "N/A",
                    TotalContribution = u.UserContributions
                        .Where(c => !c.IsDeleted)
                        .Sum(c => c.Amount),
                    TotalRepaidAmount = 0,
                    CopyOfCurrentBreakdownAtOnboarding = u.UserContributionScheme != null ?
                        u.UserContributionScheme.CopyOfCurrentBreakdownAtOnboarding : null,
                })
                .ToListAsync(cancellation);

            return new PagedList<AppUserAdmin>(items, totalCount, @params.PageNumber, @params.PageSize);
        }
    }
}


namespace CirclesFundMe.Infrastructure.Persistence.Repositories.AdminPortal
{
    public class UserManagementRepository(SqlDbContext context) : IUserManagementRepository
    {
        private readonly SqlDbContext _context = context;

        public async Task<bool> DeactivateUser(string userId, CancellationToken cancellation)
        {
            AppUser? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.UserContributionScheme != null, cancellation);

            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync(cancellation) > 0;
        }

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
                    query = query.Where(u =>
                            u.IsDeleted == false
                            && u.UserKYC != null
                                && !string.IsNullOrEmpty(u.UserKYC.BVN)
                                && u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.Selfie)
                                && u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.UtilityBill)
                                && u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.GovernmentIssuedId)
                                );
                    break;
                case AdminUserStatus.Deactivated:
                    query = query.Where(u => u.IsDeleted);
                    break;
                case AdminUserStatus.PendingKYC:
                    query = query.Where(u =>
                            u.UserKYC == null
                            || string.IsNullOrEmpty(u.UserKYC.BVN)
                            || !u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.Selfie)
                            || !u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.UtilityBill)
                            || !u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.GovernmentIssuedId)
                        );
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
                    IsDeleted = u.IsDeleted
                })
                .ToListAsync(cancellation);

            return new PagedList<AppUserAdmin>(items, totalCount, @params.PageNumber, @params.PageSize);
        }

        public async Task<IEnumerable<AppUser>> GetUsersByCommunicationTarget(CommunicationTarget target)
        {
            IQueryable<AppUser> query = _context.Users
                .AsNoTracking();

            switch (target)
            {
                case CommunicationTarget.All:
                    query = query.Where(u => !u.IsDeleted);
                    break;
                case CommunicationTarget.PendingKYCMembers:
                    query = query.Where(u =>
                            u.UserKYC == null
                            || string.IsNullOrEmpty(u.UserKYC.BVN)
                            || !u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.Selfie)
                            || !u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.UtilityBill)
                            || !u.UserDocuments.Any(d => d.DocumentType == UserDocumentTypeEnums.GovernmentIssuedId)
                    );
                    break;
                case CommunicationTarget.ActiveBorrowers:
                    break;
                case CommunicationTarget.OverdueRepaymentMembers:
                    break;
                default:
                    query = query.Where(u => u.IsDeleted);
                    break;
            }

            return await query
                .Select(u => new AppUser
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                })
                .ToListAsync();
        }

        public async Task<bool> ReactivateUser(string userId, CancellationToken cancellation)
        {
            AppUser? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.UserContributionScheme != null, cancellation);

            if (user == null)
            {
                return false;
            }

            user.IsDeleted = false;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync(cancellation) > 0;
        }
    }
}

﻿namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Users
{
    public class UserRepository(SqlDbContext context) : IUserRepository
    {
        private readonly SqlDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<AppUserExtension?> GetUserByIdAsync(string id, CancellationToken cancellation)
        {
            AppUserExtension? user = await _context.Users
                .AsNoTrackingWithIdentityResolution()
                .Where(u => u.Id == id && !u.IsDeleted)
                .Select(u => new AppUserExtension
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    UserType = u.UserType,
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    AllowPushNotifications = u.AllowPushNotifications,
                    AllowEmailNotifications = u.AllowEmailNotifications,
                    OnboardingStatus = u.OnboardingStatus,
                    IsPaymentSetupComplete = u.IsPaymentSetupComplete,
                    DateOfBirth = u.DateOfBirth,
                    Gender = u.Gender,
                    UserContributionScheme = u.UserContributionScheme == null ? null : new UserContributionScheme
                    {
                        Id = u.UserContributionScheme.Id,
                        ContributionScheme = new()
                        {
                            Id = u.UserContributionScheme.ContributionScheme!.Id,
                            Name = u.UserContributionScheme.ContributionScheme.Name,
                        },
                        ContributionAmount = u.UserContributionScheme.ContributionAmount,
                        IncomeAmount = u.UserContributionScheme.IncomeAmount
                    },
                    WithdrawalSetting = u.WithdrawalSetting == null ? null : new UserWithdrawalSetting
                    {
                        Id = u.WithdrawalSetting.Id,
                        AccountNumber = u.WithdrawalSetting.AccountNumber,
                        AccountName = u.WithdrawalSetting.AccountName,
                        BankCode = u.WithdrawalSetting.BankCode
                    },
                })
                .FirstOrDefaultAsync(cancellation);

            return user;
        }

        public async Task<AppUser?> GetUserByIdMiniAsync(string id, CancellationToken cancellation)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == id && !u.IsDeleted)
                .FirstOrDefaultAsync(cancellation);
        }

        public async Task<PagedList<AppUser>> GetUsersAsync(UserParams userParams, CancellationToken cancellation)
        {
            IQueryable<AppUser> query = _context.Users.AsNoTracking();

            if (!string.IsNullOrEmpty(userParams.SearchKey))
            {
                query = query.Where(m => m.FirstName.Contains(userParams.SearchKey)
                || m.LastName.Contains(userParams.SearchKey));
            }

            if (Enum.IsDefined(userParams.UserType))
            {
                query = query.Where(u => u.UserType == userParams.UserType);
            }

            return await PagedList<AppUser>.ToPagedListAsync(query.OrderBy(u => u.FirstName), userParams.PageNumber, userParams.PageSize, cancellation);
        }
    }
}

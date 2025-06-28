namespace CirclesFundMe.Infrastructure.Persistence
{
    public class SeedData(ILogger<SeedData> logger)
    {
        private readonly ILogger<SeedData> _logger = logger;

        public async Task Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                var dbContext = serviceProvider.GetRequiredService<SqlDbContext>();

                // Ensure the database is created.
                await dbContext.Database.EnsureCreatedAsync();

                // Initialize
                await InitializeRoles(serviceProvider);
                await InitializeAccount(serviceProvider);
                await InitializeDefaults(serviceProvider);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during data initialization: {ex.Message}\nInner Exception: {ex.InnerException}\n");
            }
        }

        private static async Task InitializeRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            List<string> roles =
            [
                Roles.SuperAdmin,
                Roles.Admin,
                Roles.Member,
            ];

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
        }

        private static async Task InitializeAccount(IServiceProvider serviceProvider)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            SqlDbContext dbContext = serviceProvider.GetRequiredService<SqlDbContext>();

            CFMAccount account = new()
            {
                Id = Guid.Parse("82bd43c1-683a-4f66-accb-b68760606a45"),
                Name = "Circles Fund Me",
                AccountType = AccountTypeEnums.Admin,
                AccountStatus = AccountStatusEnums.Active,
                CreatedBy = "System",
            };

            if (await dbContext.CFMAccounts.FindAsync(account.Id) == null)
            {
                await dbContext.CFMAccounts.AddAsync(account);
                await dbContext.SaveChangesAsync();
            }

            List<AppUser> users =
            [
                new AppUser
                {
                    FirstName = "Dev",
                    LastName = "CirclesFundMe",
                    MiddleName = "",
                    DateOfBirth = new DateTime(1995, 10, 21),
                    Email = "dev.circlesfundme@gmail.com",
                    PhoneNumber = "08144001908",
                    UserName = "dev.circlesfundme@gmail.com",
                    EmailConfirmed = true,
                    CFMAccountId = account.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System",
                    UserType = UserTypeEnums.Staff,
                    TimeZone = "Africa/Lagos",
                    OnboardingStatus = OnboardingStatusEnums.Completed,
                }
            ];

            foreach (AppUser user in users)
            {
                if (await userManager.FindByEmailAsync(user.Email!) == null)
                {
                    IdentityResult result = await userManager.CreateAsync(user, "Test@1234");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRolesAsync(user, [Roles.Admin, Roles.SuperAdmin, Roles.Member]);
                    }
                }
            }
        }

        private static async Task InitializeDefaults(IServiceProvider serviceProvider)
        {
            SqlDbContext dbContext = serviceProvider.GetRequiredService<SqlDbContext>();

            // Ensure default contribution schemes exist
            List<ContributionScheme> contributionSchemes =
            [
                new ContributionScheme
                {
                    Id = Guid.Parse("a803034d-5f72-45e2-91fc-a2979d74852c"),
                    Name = "Weekly Contribution Scheme",
                    Description = "A weekly contribution scheme where members contribute weekly.",
                    SchemeType = SchemeTypeEnums.Weekly,
                    ContributionPercent = 20.0,
                    EligibleLoanMultiple = 52,
                    ServiceCharge = 2500.0,
                    LoanManagementFee = 6.0,
                    DefaultPenaltyPercent = 25.0,
                },
                new ContributionScheme
                {
                    Id = Guid.Parse("b803034d-5f72-45e2-91fc-a2979d74852c"),
                    Name = "Monthly Contribution Scheme",
                    Description = "A monthly contribution scheme where members contribute monthly.",
                    SchemeType = SchemeTypeEnums.Monthly,
                    ContributionPercent = 20,
                    EligibleLoanMultiple = 12,
                    ServiceCharge = 2500.0,
                    LoanManagementFee = 6.0,
                    DefaultPenaltyPercent = 25.0,
                },
                new ContributionScheme
                {
                    Id = Guid.Parse("c803034d-5f72-45e2-91fc-a2979d74852c"),
                    Name = "Asset Finance",
                    Description = "A contribution to help you get asset loans",
                    SchemeType = SchemeTypeEnums.AssetFinance,
                    EquityPercent = 10.0,
                    LoanTerm = 208.0,
                    LoanManagementFee = 6.0,
                    PreLoanServiceChargePercent = 0.025,
                    PostLoanServiceChargePercent = 0.025,
                }
            ];

            foreach (ContributionScheme scheme in contributionSchemes)
            {
                if (await dbContext.ContributionSchemes.FindAsync(scheme.Id) == null)
                {
                    await dbContext.ContributionSchemes.AddAsync(scheme);
                    await dbContext.SaveChangesAsync();
                }
            }

            return;
        }
    }
}

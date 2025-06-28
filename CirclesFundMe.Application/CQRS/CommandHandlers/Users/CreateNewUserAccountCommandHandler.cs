namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class CreateNewUserAccountCommandHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IOTPService oTPService) : IRequestHandler<CreateNewUserAccountCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IOTPService _oTPService = oTPService;

        public async Task<BaseResponse<bool>> Handle(CreateNewUserAccountCommand request, CancellationToken cancellationToken)
        {
            CFMAccount account = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Email ?? "Default Account Name",
                AccountType = AccountTypeEnums.Member,
                AccountStatus = AccountStatusEnums.Pending
            };

            try
            {
                (bool isOtpValid, string message) = await _oTPService.ValidateOtp(request.Email!, request.OTP!, cancellationToken);

                if (!isOtpValid)
                {
                    return BaseResponse<bool>.BadRequest(message);
                }

                await _unitOfWork.Accounts.AddAsync(account, cancellationToken);
                bool accountCreated = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

                if (accountCreated)
                {
                    AppUser user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = request.Email,
                        Email = request.Email,
                        FirstName = "Default",
                        LastName = "User",
                        UserType = UserTypeEnums.Member,
                        TimeZone = "Africa/Lagos",
                        OnboardingStatus = OnboardingStatusEnums.InProgress,
                        AgreedToTerms = true,
                        CFMAccountId = account.Id,
                        EmailConfirmed = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = request.Email
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, request.Password!);

                    if (result.Succeeded)
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, Roles.Member);
                        if (!roleResult.Succeeded)
                        {
                            _unitOfWork.Accounts.Delete(account);
                            await _unitOfWork.SaveChangesAsync(cancellationToken);

                            await _userManager.DeleteAsync(user);

                            return BaseResponse<bool>.BadRequest(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                        }

                        return BaseResponse<bool>.Success(true, "Account created successfully.");
                    }
                    else
                    {
                        _unitOfWork.Accounts.Delete(account);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);

                        return BaseResponse<bool>.BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                return BaseResponse<bool>.BadRequest("Failed to create account. Please try again.");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.BadRequest($"Error creating account: {ex.Message}");
            }
        }
    }
}

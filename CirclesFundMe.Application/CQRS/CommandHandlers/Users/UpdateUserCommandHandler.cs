namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, UserManager<AppUser> userManager) : IRequestHandler<UpdateUserCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<BaseResponse<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (user == null)
            {
                return BaseResponse<bool>.NotFound("User not found.");
            }

            _mapper.Map(request, user);
            user.UpdateAuditFields(_currentUserService.UserId);
            await _userManager.UpdateAsync(user);

            if (request.ContributionSchemeId.HasValue)
            {
                UserContributionScheme? userContributionScheme = await _unitOfWork.UserContributionSchemes.GetOneAsync([x => x.UserId == _currentUserService.UserId], cancellationToken);
                if (userContributionScheme == null)
                {
                    return BaseResponse<bool>.NotFound("User contribution scheme not found.");
                };

                ContributionScheme? contributionScheme = await _unitOfWork.ContributionSchemes.GetByPrimaryKey(request.ContributionSchemeId.Value, cancellationToken);
                if (contributionScheme == null)
                {
                    return BaseResponse<bool>.NotFound("Contribution scheme not found.");
                }

                if (contributionScheme.SchemeType != SchemeTypeEnums.AutoFinance && request.IncomeAmount == null)
                {
                    return BaseResponse<bool>.BadRequest("Income amount is required for this contribution scheme type.");
                }

                if (contributionScheme.SchemeType == SchemeTypeEnums.Weekly || contributionScheme.SchemeType == SchemeTypeEnums.Monthly)
                {
                    if (request.ContributionAmount > ((decimal)contributionScheme.ContributionPercent / 100) * request.IncomeAmount)
                    {
                        return BaseResponse<bool>.BadRequest($"Contribution amount cannot be more than {contributionScheme.ContributionPercent}% of your income.");
                    }

                    userContributionScheme.IncomeAmount = request.IncomeAmount!.Value;
                }

                userContributionScheme.ContributionSchemeId = contributionScheme.Id;
                userContributionScheme.ContributionAmount = request.ContributionAmount!.Value;
                userContributionScheme.UpdateAudit(_currentUserService.UserId);

                _unitOfWork.UserContributionSchemes.Update(userContributionScheme);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.Success(true, "User updated successfully.");
        }
    }
}

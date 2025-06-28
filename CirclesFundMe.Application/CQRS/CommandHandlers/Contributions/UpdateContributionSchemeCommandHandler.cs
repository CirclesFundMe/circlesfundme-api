namespace CirclesFundMe.Application.CQRS.CommandHandlers.Contributions
{
    public class UpdateContributionSchemeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<UpdateContributionSchemeCommand, BaseResponse<ContributionSchemeModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<ContributionSchemeModel>> Handle(UpdateContributionSchemeCommand request, CancellationToken cancellationToken)
        {
            ContributionScheme? contributionScheme = await _unitOfWork.ContributionSchemes.GetByPrimaryKey(request.ContributionSchemeId, cancellationToken);

            if (contributionScheme == null)
            {
                return BaseResponse<ContributionSchemeModel>.NotFound("Contribution scheme not found.");
            }

            _mapper.Map(request, contributionScheme);
            contributionScheme.UpdateAudit(_currentUserService.UserId);

            _unitOfWork.ContributionSchemes.Update(contributionScheme);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            ContributionSchemeModel contributionSchemeModel = _mapper.Map<ContributionSchemeModel>(contributionScheme);

            return BaseResponse<ContributionSchemeModel>.Success(contributionSchemeModel, "Contribution scheme updated successfully.");
        }
    }
}

namespace CirclesFundMe.Application.CQRS.QueryHandlers.Contributions
{
    public class GetContributionSchemesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetContributionSchemesQuery, BaseResponse<List<ContributionSchemeModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BaseResponse<List<ContributionSchemeModel>>> Handle(GetContributionSchemesQuery request, CancellationToken cancellationToken)
        {
            List<ContributionScheme> contributionSchemes = await _unitOfWork.ContributionSchemes.GetContributionSchemes(cancellationToken);

            return BaseResponse<List<ContributionSchemeModel>>.Success(_mapper.Map<List<ContributionSchemeModel>>(contributionSchemes));
        }
    }
}

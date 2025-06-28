namespace CirclesFundMe.Application.CQRS.QueryHandlers.Contributions
{
    public class GetContributionSchemesMiniQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetContributionSchemesMiniQuery, BaseResponse<List<ContributionSchemeMiniModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<List<ContributionSchemeMiniModel>>> Handle(GetContributionSchemesMiniQuery request, CancellationToken cancellationToken)
        {
            List<ContributionScheme> contributionSchemes = await _unitOfWork.ContributionSchemes.GetContributionSchemesMini(cancellationToken);

            List<ContributionSchemeMiniModel> contributionSchemeModels = contributionSchemes
                .Select(cs => new ContributionSchemeMiniModel
                {
                    Id = cs.Id,
                    Name = cs.Name,
                    Type = cs.SchemeType.ToString()
                })
                .ToList();

            return BaseResponse<List<ContributionSchemeMiniModel>>.Success(contributionSchemeModels);
        }
    }
}

namespace CirclesFundMe.Application.CQRS.QueryHandlers.Contributions
{
    public class GetContributionSchemesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetContributionSchemesQuery, BaseResponse<List<ContributionSchemeModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<List<ContributionSchemeModel>>> Handle(GetContributionSchemesQuery request, CancellationToken cancellationToken)
        {
            List<ContributionScheme> contributionSchemes = await _unitOfWork.ContributionSchemes.GetContributionSchemesAsync(cancellationToken);

            List<ContributionSchemeModel> contributionSchemeModels = contributionSchemes
                .Select(cs => new ContributionSchemeModel
                {
                    Id = cs.Id,
                    Name = cs.Name,
                    Type = cs.SchemeType.ToString()
                })
                .ToList();

            return BaseResponse<List<ContributionSchemeModel>>.Success(contributionSchemeModels);
        }
    }
}

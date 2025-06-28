namespace CirclesFundMe.Application.CQRS.Queries.Contributions
{
    public record GetContributionSchemesMiniQuery : IRequest<BaseResponse<List<ContributionSchemeMiniModel>>>
    {
    }
}

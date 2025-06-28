namespace CirclesFundMe.Application.CQRS.Queries.Contributions
{
    public record GetContributionSchemesQuery : IRequest<BaseResponse<List<ContributionSchemeModel>>>
    {
    }
}

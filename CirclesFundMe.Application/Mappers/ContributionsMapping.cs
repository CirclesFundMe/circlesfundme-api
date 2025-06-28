namespace CirclesFundMe.Application.Mappers
{
    public class ContributionsMapping : Profile
    {
        public ContributionsMapping()
        {
            CreateMap<ContributionScheme, ContributionSchemeModel>().ReverseMap();
            CreateMap<ContributionScheme, UpdateContributionSchemeCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => UtilityHelper.ShouldMapMember(srcMember)));
        }
    }
}

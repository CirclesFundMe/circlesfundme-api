namespace CirclesFundMe.Application.Mappers
{
    public class ContributionsMapping : Profile
    {
        public ContributionsMapping()
        {
            CreateMap<ContributionScheme, ContributionSchemeModel>().ReverseMap();
        }
    }
}

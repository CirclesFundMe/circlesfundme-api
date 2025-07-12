namespace CirclesFundMe.Application.Mappers
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, UserModel>().ReverseMap();
            CreateMap<AppUser, UpdateUserCommand>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => UtilityHelper.ShouldMapMember(srcMember)));
        }
    }
}

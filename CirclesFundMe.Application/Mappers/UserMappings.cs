namespace CirclesFundMe.Application.Mappers
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, UserModel>().ReverseMap();
        }
    }
}

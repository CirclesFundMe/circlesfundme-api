namespace CirclesFundMe.Application.CQRS.Queries.Users
{
    public record GetMyProfileQuery : IRequest<BaseResponse<UserModel>>
    {
    }
}

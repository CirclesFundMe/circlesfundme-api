namespace CirclesFundMe.Application.CQRS.Queries.Users
{
    public record GetUsersQuery : IRequest<BaseResponse<PagedList<UserModel>>>
    {
        public required UserParams Params { get; set; }
    }
}

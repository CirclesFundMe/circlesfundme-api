namespace CirclesFundMe.Application.CQRS.Queries.Users
{
    public record GetUserByIdQuery : IRequest<BaseResponse<UserModel>>
    {
        public required string UserId { get; set; }
    }
}

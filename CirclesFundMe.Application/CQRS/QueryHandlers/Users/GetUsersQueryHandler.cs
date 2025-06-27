namespace CirclesFundMe.Application.CQRS.QueryHandlers.Users
{
    public class GetUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUsersQuery, BaseResponse<PagedList<UserModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BaseResponse<PagedList<UserModel>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            PagedList<AppUser> users = await _unitOfWork.Users.GetUsersAsync(request.Params, cancellationToken);

            return new()
            {
                Data = PagedListHelper<UserModel>.MapToList(users, _mapper),
                MetaData = PagedListHelper<AppUser>.GetPaginationInfo(users),
            };
        }
    }
}

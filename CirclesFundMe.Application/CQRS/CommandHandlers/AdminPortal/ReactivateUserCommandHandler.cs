using CirclesFundMe.Application.CQRS.Commands.AdminPortal;

namespace CirclesFundMe.Application.CQRS.CommandHandlers.AdminPortal
{
    public class ReactivateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ReactivateUserCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(ReactivateUserCommand request, CancellationToken cancellationToken)
        {
            bool result = await _unitOfWork.UserManagement.ReactivateUser(request.UserId, cancellationToken);

            return result
                ? BaseResponse<bool>.Success(true, "User reactivated successfully.")
                : BaseResponse<bool>.BadRequest("Failed to reactivate user.");
        }
    }
}

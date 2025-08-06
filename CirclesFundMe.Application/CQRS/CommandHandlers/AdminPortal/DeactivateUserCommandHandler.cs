using CirclesFundMe.Application.CQRS.Commands.AdminPortal;

namespace CirclesFundMe.Application.CQRS.CommandHandlers.AdminPortal
{
    public class DeactivateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeactivateUserCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            bool result = await _unitOfWork.UserManagement.DeactivateUser(request.UserId, cancellationToken);

            return result
                ? BaseResponse<bool>.Success(true, "User deactivated successfully.")
                : BaseResponse<bool>.BadRequest("Failed to deactivate user.");
        }
    }
}

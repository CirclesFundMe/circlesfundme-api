
namespace CirclesFundMe.Application.CQRS.CommandHandlers.Loans
{
    public class RejectLoanApplicationCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<RejectLoanApplicationCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<bool>> Handle(RejectLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            LoanApplication? loanApplication = await _unitOfWork.LoanApplications.GetByPrimaryKey(request.Id, cancellationToken);

            if (loanApplication == null)
            {
                return BaseResponse<bool>.NotFound("Loan application not found.");
            }

            if (loanApplication.Status != LoanApplicationStatusEnums.Pending)
            {
                return BaseResponse<bool>.BadRequest("Only pending loan applications can be rejected.");
            }

            loanApplication.Status = LoanApplicationStatusEnums.Rejected;
            loanApplication.RejectionReason = request.RejectionReason;
            loanApplication.ModifiedDate = DateTime.UtcNow;
            loanApplication.ModifiedBy = _currentUserService.UserId;

            _unitOfWork.LoanApplications.Update(loanApplication);
            bool result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (result)
            {
                return BaseResponse<bool>.Success(true, "Loan application has been successfully rejected.");
            }
            else
            {
                return BaseResponse<bool>.BadRequest("An error occurred while rejecting the loan application.");
            }
        }
    }
}

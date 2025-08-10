namespace CirclesFundMe.Application.CQRS.CommandHandlers.Loans
{
    public class WaitlistLoanApplicationCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<WaitlistLoanApplicationCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<bool>> Handle(WaitlistLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            LoanApplication? loanApplication = await _unitOfWork.LoanApplications.GetByPrimaryKey(request.Id, cancellationToken);

            if (loanApplication == null)
            {
                return BaseResponse<bool>.NotFound("Loan application not found.");
            }

            if (loanApplication.Status != LoanApplicationStatusEnums.Pending)
            {
                return BaseResponse<bool>.BadRequest("Only pending loan applications can be waitlisted.");
            }

            loanApplication.Status = LoanApplicationStatusEnums.Waitlist;
            loanApplication.ModifiedDate = DateTime.UtcNow;
            loanApplication.ModifiedBy = _currentUserService.UserId;

            _unitOfWork.LoanApplications.Update(loanApplication);
            bool result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (result)
            {
                return BaseResponse<bool>.Success(true, "Loan application has been successfully waitlisted.");
            }
            else
            {
                return BaseResponse<bool>.BadRequest("An error occurred while waitlisting the loan application.");
            }
        }
    }
}

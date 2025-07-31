namespace CirclesFundMe.Application.CQRS.CommandHandlers.Loans
{
    public class CreateLoanApplicationCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<CreateLoanApplicationCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly string _currentUserId = currentUserService.UserId;

        public async Task<BaseResponse<bool>> Handle(CreateLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            LoanApplication? loanApplication = await _unitOfWork.LoanApplications.GetOneAsync([x => x.UserId == _currentUserId,
                x => x.Status != LoanApplicationStatusEnums.Approved], cancellationToken);

            if (loanApplication != null)
            {
                return BaseResponse<bool>.BadRequest("You already have an unapproved loan application.");
            }

            loanApplication = new LoanApplication
            {
                UserId = _currentUserId,
                Status = LoanApplicationStatusEnums.Pending,
                ApprovedAmount = 0
            };

            await _unitOfWork.LoanApplications.AddAsync(loanApplication, cancellationToken);
            bool isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (isSaved)
            {
                return BaseResponse<bool>.Success(true, "Loan application created successfully.");
            }
            else
            {
                return BaseResponse<bool>.BadRequest("Failed to create loan application. Our engineers are investigating this issue");
            }
        }
    }
}

namespace CirclesFundMe.Application.CQRS.QueryHandlers.Loans
{
    public class GetLoanApplicationsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetLoanApplicationsQuery, BaseResponse<PagedList<LoanApplicationModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<PagedList<LoanApplicationModel>>> Handle(GetLoanApplicationsQuery request, CancellationToken cancellationToken)
        {
            PagedList<LoanApplicationExtension> loanApplications = await _unitOfWork.LoanApplications.GetLoanApplications(request.LoanApplicationParams, cancellationToken);

            List<LoanApplicationModel> loanApplicationModels = loanApplications.Select(la => new LoanApplicationModel
            {
                Id = la.Id,
                Status = la.Status.ToString(),
                Scheme = la.Scheme.ToString(),
                RequestedAmount = la.RequestedAmount,
                EligibleLoanAmount = la.CurrentEligibleAmount,
                AmountRepaid = la.AmountRepaid,
                ApprovedAmount = la.ApprovedAmount,
                DateApplied = la.CreatedDate,
                ApplicantDetail = new LoanApplicantDetail
                {
                    FirstName = la.User?.FirstName,
                    LastName = la.User?.LastName,
                    ImageUrl = la.User?.ProfilePictureUrl
                }
            }).ToList();

            PagedList<LoanApplicationModel> pagedLoanApplicationModels = new(loanApplicationModels, loanApplications.TotalCount, loanApplications.CurrentPage, loanApplications.PageSize);

            return new BaseResponse<PagedList<LoanApplicationModel>>
            {
                Data = pagedLoanApplicationModels,
                MetaData = PagedListHelper<LoanApplicationModel>.GetPaginationInfo(pagedLoanApplicationModels)
            };
        }
    }
}

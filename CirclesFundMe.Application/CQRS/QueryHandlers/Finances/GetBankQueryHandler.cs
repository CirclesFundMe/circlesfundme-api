
namespace CirclesFundMe.Application.CQRS.QueryHandlers.Finances
{
    public class GetBankQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBankQuery, BaseResponse<IEnumerable<BankModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<IEnumerable<BankModel>>> Handle(GetBankQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.Finances.Bank> banks = await _unitOfWork.Banks.GetBanks(cancellationToken);

            return new()
            {
                Data = banks.Select(b => new BankModel
                {
                    BankCode = b.Code,
                    BankName = b.Name
                }),
            };
        }
    }
}

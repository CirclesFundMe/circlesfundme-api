﻿
namespace CirclesFundMe.Application.CQRS.QueryHandlers.Finances
{
    public class GetMyWalletsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<GetMyWalletsQuery, BaseResponse<List<WalletModel>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BaseResponse<List<WalletModel>>> Handle(GetMyWalletsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Wallet> wallets = await _unitOfWork.Wallets.GetMyWallets(_currentUserService.UserId, cancellationToken);

            List<WalletModel> walletModels = wallets.Select(wallet => new WalletModel
            {
                Id = wallet.Id,
                Title = wallet.Type == WalletTypeEnums.Contribution ? "Your contribution" : "Maximum Loan Eligible",
                Balance = UtilityHelper.FormatDecimalToNairaWithSymbol(wallet.Balance),
                Scheme = GetSchemeDisplayName(wallet.User!.UserContributionScheme!.ContributionScheme!.SchemeType),
                Action = wallet.Type == WalletTypeEnums.Contribution ? "Withdraw" : "Apply for Loan",
                NextTranDate = wallet.NextTranDate?.ToString("d MMMM, yyyy")
            }).ToList();

            return BaseResponse<List<WalletModel>>.Success(walletModels, "Wallets retrieved successfully.");
        }

        private static string GetSchemeDisplayName(SchemeTypeEnums schemeType)
        {
            return schemeType switch
            {
                SchemeTypeEnums.Weekly => "Weekly Contribution",
                SchemeTypeEnums.Monthly => "Monthly Contribution",
                SchemeTypeEnums.AutoFinance => "Auto Finance Contribution",
                _ => "Unknown Scheme"
            };
        }
    }
}

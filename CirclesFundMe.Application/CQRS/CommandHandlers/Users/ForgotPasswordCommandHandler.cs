namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IOTPService oTPService) : IRequestHandler<ForgotPasswordCommand, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IOTPService _oTPService = oTPService;

        public async Task<BaseResponse<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            string email = UtilityHelper.NormalizeLower(request.Email);

            AppUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BaseResponse<string>.NotFound($"User with email {email} not found");
            }

            string otp = UtilityHelper.GenerateOtp();

            (bool result, string message) = await _oTPService.SendOtp(email, otp, user.FirstName, cancellationToken);

            if (!result)
            {
                return BaseResponse<string>.BadRequest(message);
            }

            return new()
            {
                Data = message
            };
        }
    }
}

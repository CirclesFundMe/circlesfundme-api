namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class LoginCommandHandler(IJwtService jwtService, UserManager<AppUser> userManager) : IRequestHandler<LoginCommand, BaseResponse<LoginModel>>
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<BaseResponse<LoginModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            string email = UtilityHelper.NormalizeLower(request.Email);

            AppUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BaseResponse<LoginModel>.Unauthorized("Incorrect email or password");
            }

            bool passwordIsCorrect = await _userManager.CheckPasswordAsync(user, request.Password!);

            if (!passwordIsCorrect)
            {
                return BaseResponse<LoginModel>.Unauthorized("Incorrect email or password");
            }

            if (!user.EmailConfirmed)
            {
                return BaseResponse<LoginModel>.Unauthorized("Please verify your email address");
            }

            LoginModel loginResponse = await _jwtService.GenerateLoginResponse(user);

            return new()
            {
                Message = "Login was successful",
                Data = loginResponse
            };
        }
    }
}

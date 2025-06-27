namespace CirclesFundMe.Application.CQRS.CommandHandlers.Users
{
    public class RefreshTokenCommandHandler(IJwtService jwtService, UserManager<AppUser> userManager) : IRequestHandler<RefreshTokenCommand, BaseResponse<LoginModel>>
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<BaseResponse<LoginModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal? principal = _jwtService.GetPrincipalFromExpiredToken(request.ExpiredToken);

            if (principal == null)
            {
                return BaseResponse<LoginModel>.BadRequest("Unable to process token refresh request");
            }

            string? userId = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (userId == null)
            {
                return BaseResponse<LoginModel>.BadRequest("Unable to process token refresh request");
            }

            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BaseResponse<LoginModel>.BadRequest("Refresh token is invalid, please login again");
            }

            LoginModel loginResponse = await _jwtService.GenerateLoginResponse(user);

            return new()
            {
                Message = "Token refresh was successful",
                Data = loginResponse
            };
        }
    }
}

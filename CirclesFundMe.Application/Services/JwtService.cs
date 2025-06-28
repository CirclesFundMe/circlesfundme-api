namespace CirclesFundMe.Application.Services
{
    public interface IJwtService
    {
        (string token, DateTime expiry) GenerateAccessToken(List<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        Task<LoginModel> GenerateLoginResponse(AppUser user);
    }
    public record JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly SymmetricSecurityKey _securityKey;
        public JwtService(IConfiguration config, UserManager<AppUser> userManager, SqlDbContext context)
        {
            _config = config;
            _userManager = userManager;
            _securityKey = new(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"] ?? "super-secret-word"));
        }

        public (string token, DateTime expiry) GenerateAccessToken(List<Claim> claims)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SigningCredentials signingCredentials = GetSigningCredentials();
            (SecurityTokenDescriptor tokenOptions, DateTime tokenExpiry) = GenerateTokenOptions(signingCredentials, claims);

            SecurityToken token = tokenHandler.CreateToken(tokenOptions);
            return (tokenHandler.WriteToken(token), tokenExpiry);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<LoginModel> GenerateLoginResponse(AppUser user)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);
            await _userManager.UpdateAsync(user);

            List<Claim> claims =
            [
                new(ClaimTypes.Email, user.Email!),
                new("userId", user.Id),
                new("accountId", user.CFMAccountId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            foreach (string role in roles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }

            (string token, DateTime expiry) = GenerateAccessToken(claims);

            return new LoginModel
            {
                Email = user.Email!,
                UserId = user.Id,
                AccessToken = token,
                Expiry = expiry,
                RefreshToken = refreshToken,
                Role = roles.First(),
                OnboardingStatus = user.OnboardingStatus.ToString()
            };
        }

        private (SecurityTokenDescriptor, DateTime tokenExpiry) GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            SecurityTokenDescriptor tokenOptions = new()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _config["JwtSettings:ValidIssuer"],
                Audience = _config["JwtSettings:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JwtSettings:Expires"])),
                SigningCredentials = signingCredentials,
            };

            return (tokenOptions, tokenOptions.Expires ?? DateTime.UtcNow.AddMinutes(10));
        }

        private SigningCredentials GetSigningCredentials()
        {
            return new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateIssuer = true,
                ValidIssuer = _config["JwtSettings:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = _config["JwtSettings:ValidAudience"],
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokenHandler = new();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}

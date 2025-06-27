namespace CirclesFundMe.Application.Services
{
    public interface ICurrentUserService
    {
        string UserEmail { get; }
        string UserId { get; }
        string AccountId { get; }
        string UserIpAddress { get; }
        bool IsAdmin { get; }
        bool IsMember { get; }
        string Role { get; }
    }
    public record CurrentUserService(IHttpContextAccessor HttpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = HttpContextAccessor;

        public string UserEmail =>
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
            ?? throw new FormatException("Current user email not encrypted in token");

        public string UserId =>
            _httpContextAccessor.HttpContext?.User.FindFirstValue("userId")
            ?? throw new FormatException("Current user id not encrypted in token");

        public string AccountId =>
            _httpContextAccessor.HttpContext?.User.FindFirstValue("accountId")
            ?? throw new FormatException("Current user account id not encrypted in token");

        public string UserIpAddress =>
            _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault()
            ?? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
            ?? throw new FormatException("Current user Ip address is unknown");

        public bool IsAdmin =>
            _httpContextAccessor.HttpContext?.User.IsInRole(Roles.Admin.ToString())
            ?? throw new FormatException("Current user role not encrypted in token");

        public bool IsMember =>
            _httpContextAccessor.HttpContext?.User.IsInRole(Roles.Member.ToString())
            ?? throw new FormatException("Current user role not encrypted in token");

        public string Role =>
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)
            ?? throw new FormatException("Current user role not encrypted in token");
    }
}

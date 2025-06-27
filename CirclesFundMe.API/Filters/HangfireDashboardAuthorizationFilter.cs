namespace CirclesFundMe.API.Filters
{
    public class HangfireDashboardAuthorizationFilter(IConfiguration config) : IDashboardAuthorizationFilter
    {
        private readonly bool _isProduction = config.GetValue<bool>("AppSettings:IsProduction");

        public bool Authorize([NotNull] DashboardContext context)
        {
            if (_isProduction)
            {
                return false;
            }

            return true;
        }
    }
}

namespace CirclesFundMe.API.Filters
{
    public class SecurityHeadersStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Use(async (context, next) =>
                {
                    PathString path = context.Request.Path;

                    if (!path.StartsWithSegments("/swagger") && !path.StartsWithSegments("/api-docs"))
                    {
                        context.Response.OnStarting(() =>
                        {
                            IHeaderDictionary headers = context.Response.Headers;

                            headers.XFrameOptions = "DENY";
                            headers.CacheControl = "no-store, no-cache, must-revalidate";
                            headers.XXSSProtection = "1; mode=block";
                            headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains";
                            headers.XContentTypeOptions = "nosniff";
                            headers.ContentSecurityPolicy = "default-src 'self'";
                            headers["Referrer-Policy"] = "no-referrer";
                            headers["Permissions-Policy"] = "geolocation=(), camera=()";
                            headers.Server = "DENY";

                            headers["Expect-CT"] = "max-age=86400, enforce";
                            headers["X-Permitted-Cross-Domain-Policies"] = "none";

                            // Remove server identification headers
                            headers.Remove("Server");
                            headers.Remove("X-Powered-By");
                            headers.Remove("X-AspNet-Version");

                            return Task.CompletedTask;
                        });
                    }

                    await next();
                });

                next(app);
            };
        }
    }
}

namespace CirclesFundMe.API
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration config)
        {
            #region Swagger Setup
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "CirclesFundMe.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                c.OperationFilter<ApplySummariesOperationFilter>();
                c.SchemaFilter<EnumSchemaFilter>();
                c.EnableAnnotations();
            });
            #endregion

            #region API Versioning Setup
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("api-version"));
            });
            #endregion

            #region Response Compression for Better Performance
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Optimal;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest;
            });

            services.Configure<ResponseCompressionOptions>(options =>
            {
                options.MimeTypes = ["text/plain", "text/html", "application/json"];
            });
            #endregion

            #region Security Related Region
            services.AddSingleton<IStartupFilter, SecurityHeadersStartupFilter>();
            #endregion

            #region Miscellaneous Setup
            services.AddEndpointsApiExplorer();
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationExceptionFilter>();
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            string[] origins = config["AppSettings:Origins"]?.Split(',') ?? [];
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins(origins)
                .AllowAnyMethod().AllowAnyHeader());
            });
            #endregion

            return services;
        }
    }
}

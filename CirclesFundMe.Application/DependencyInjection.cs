namespace CirclesFundMe.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            #region Microsoft Identity and Authentication
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(5);
            });

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<SqlDbContext>()
            .AddDefaultTokenProviders();

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = config["JwtSettings:ValidAudience"],
                ValidIssuer = config["JwtSettings:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"] ?? "super-secret"))
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IOTPService, OTPService>();
            #endregion

            #region Miscellaneous Injections
            services.AddOptions<AppSettings>().Bind(config.GetSection("AppSettings"));

            services.AddSingleton<UtilityHelper>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly));

            services.AddAutoMapper(typeof(UserMappings).Assembly);

            services.AddSingleton<EmailService>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            #endregion

            #region Hangfire Setup
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(config.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));
            services.AddHangfireServer();
            services.AddSingleton<IQueueService, QueueService>();
            services.AddSingleton<CFMJobs>();
            #endregion

            #region Cloudinary Setup
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddSingleton(provider =>
            {
                CloudinarySettings config = provider.GetService<IOptions<CloudinarySettings>>()!.Value;
                return new Cloudinary(new Account(
                    config.CloudName,
                    config.ApiKey,
                    config.ApiSecret
                ));
            });
            services.AddScoped<IImageService, ImageService>();
            #endregion

            #region Encryption Configure
            EncryptionSettings? encryptionSettings = config.GetSection("EncryptionSettings").Get<EncryptionSettings>();

            if (encryptionSettings == null || string.IsNullOrEmpty(encryptionSettings.Key) || string.IsNullOrEmpty(encryptionSettings.IV))
            {
                throw new Exception("Encryption settings are not configured properly.");
            }

            services.AddSingleton(new EncryptionService(encryptionSettings.Key, encryptionSettings.IV));
            #endregion

            return services;
        }
    }
}

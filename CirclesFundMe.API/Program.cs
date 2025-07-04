#region Application developed in June 2025 with .NET 8 by Henry Ozomgbachi
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddPresentationServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxResponseBufferSize = int.MaxValue;
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
});

builder.Host.UseSerilog();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint(builder.Configuration["AppSettings:SwaggerEndpoint"], string.Empty);
    s.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();
    var seedData = new SeedData(logger);
    seedData.Initialize(serviceProvider).Wait();
}

app.UseResponseCompression();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHsts();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard(builder.Configuration["AppSettings:HangfireEndpoint"], new DashboardOptions
{
    Authorization = new[] { new HangfireDashboardAuthorizationFilter(builder.Configuration) }
});

RecurringJob.AddOrUpdate<PaystackJobs>(
    "SynchronizeBanks",
    job => job.SynchronizeBanks(),
    "0 0 * * *" // Every day at midnight
);

app.Run();
#endregion
/*
 * The above code snippet is the entry point of the application. It is the Program.cs file in the CirclesFundMe.API project.
 * * The application is built using the .NET 8 framework and is developed by Henry Ozomgbachi in June 2025.
 * * The application uses Serilog for logging and Swagger for API documentation.
 * * It also includes middleware for handling global exceptions, authentication, authorization, and response compression.
 * * The application uses Hangfire for background job processing and has CORS policy configured for cross-origin requests.
 * * The application has separate services for presentation(API), application, infrastructure and domain layers.
 * * The architecture follows the CQRS pattern with MediatR for command and query handling.
 * * UnitOfWork and Repository patterns are used for data access.
 * */

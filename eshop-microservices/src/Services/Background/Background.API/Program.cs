
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.UseHangfireDashboard();
app.Run();

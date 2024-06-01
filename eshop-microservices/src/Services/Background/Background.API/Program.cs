using Hangfire;
using BuildingBlocks.Messaging.MassTransit;
using Background.API.Data;
using Microsoft.EntityFrameworkCore;
using Background.API.Settings;
using Microsoft.Extensions.Configuration;
using Background.API.Services;


var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddDbContext<HangfireDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddHangfireServer();
builder.Services.AddMessageBroker(builder.Configuration, assembly);
builder.Services.AddTransient<IEmailService, EmailService>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var hangfireDbContext = scope.ServiceProvider.GetRequiredService<HangfireDbContext>();
    hangfireDbContext.Database.Migrate();
}
app.MapGet("/", () =>  BackgroundJob.Enqueue(() =>  Console.WriteLine("Hello world")));


app.Run();

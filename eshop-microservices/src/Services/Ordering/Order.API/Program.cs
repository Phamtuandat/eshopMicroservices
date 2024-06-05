using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);



var app = builder.Build();

app.UseApiServices();
if (app.Environment.IsDevelopment()) {
    await app.InitialiseDatabaseAsync();
}

Log.Information(builder.Configuration.GetConnectionString("Database"));

app.Run();

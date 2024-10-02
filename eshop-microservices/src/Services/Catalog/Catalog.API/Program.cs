using Catalog.API.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");
// Add services to the container
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});



builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
builder.Services.AddScoped<IUnitOfWork, UnitOfwork>();
builder.Services.AddNpgsql<CatalogDbContext>(builder.Configuration.GetConnectionString("Database"));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddHttpContextAccessor();
if (builder.Environment.IsDevelopment())
{

}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    Log.Debug(builder.Configuration.GetConnectionString("Database") ?? "Empty");
}
app.UseStaticFiles();
app.UseExceptionHandler(options => { });
app.MapCarter();
await app.InitialiseDb();
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();

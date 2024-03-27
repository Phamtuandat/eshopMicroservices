using Catalog.API.Data;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
// Add services to the container
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});
builder.Services.AddCarter();
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{ 
    builder.Services.InitializeMartenWith<CatalogInitialData>();
    
}

var app = builder.Build();

app.MapCarter();
app.Run();

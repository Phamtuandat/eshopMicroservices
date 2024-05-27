using BuildingBlocks.Behaviors;
using Carter;
using Discount.Grpc.Services;
using Discount.GRPC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using BuildingBlocks.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountContext>(opts =>
        opts.UseSqlite(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMessageBroker(builder.Configuration, assembly);

builder.Services.AddAuthentication().AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:6065";
    options.Audience = "https://localhost:6065/resources"; // Ensure HTTPS is required
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateIssuerSigningKey = false,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = "https://localhost:6065/resources",
        ValidIssuer = "https://localhost:6065",
        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
        {
            var jwt = new JsonWebToken(token);

            return jwt;
        },
        ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
        $"{"https://localhost:6065"}/.well-known/openid-configuration",
        new OpenIdConnectConfigurationRetriever(),
        new HttpDocumentRetriever() { RequireHttps = false }
    )
    };
    // Enable detailed logging for diagnostics
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("admin");
    });
});
var app = builder.Build();



app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    var user = context.User;

    next.Invoke(context);
});
app.MapCarter();
// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountProtoService>();
app.Logger.LogInformation("Database is migrating!");
app.UseMigration();
app.Logger.LogInformation("Database migrated!");
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();

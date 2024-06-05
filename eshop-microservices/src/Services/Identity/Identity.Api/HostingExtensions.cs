using BuildingBlocks.Messaging.MassTransit;
using Duende.IdentityServer;
using Identity.Api.Data;
using Identity.Api.Middlewares;
using Identity.Api.Models;
using Identity.Api.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

namespace Identity.Api
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews(); // Add MVC services
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "Identity";
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddMessageBroker(builder.Configuration, typeof(Program).Assembly);
            builder.Services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>();
            builder.Services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/login";
                    options.LogoutPath = "/Account/Logout";
                })
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });

            builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddTransient<AppbarService>();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler(exceptionHandlerApp =>
                {
                    exceptionHandlerApp.Run(async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        // using static System.Net.Mime.MediaTypeNames;
                        context.Response.ContentType = Text.Plain;

                        await context.Response.WriteAsync("An exception was thrown.");

                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                        {
                            await context.Response.WriteAsync(" The file was not found.");
                        }

                        if (exceptionHandlerPathFeature?.Path == "/")
                        {
                            await context.Response.WriteAsync(" Page: Home.");
                        }
                    });
                });
            }
            app.UseRedirectMiddleware();
            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            app.UseHsts();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Map controllers

            });

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = "/contents"
            });
            return app;
        }
    }
}
﻿using Identity.Api.Data;
using Identity.Api.Enums;
using Identity.Api.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace Identity.Api
{
    public class SeedData
    {
        public static async Task EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var admin = await roleMgr.FindByNameAsync(RoleNames.Administrator);
                var staff = await roleMgr.FindByNameAsync(RoleNames.Staff);
                var user = await roleMgr.FindByNameAsync(RoleNames.User);
                if (admin == null)
                {
                    await roleMgr.CreateAsync(new IdentityRole(RoleNames.Administrator));
                    Log.Debug("admin role is created");
                }
                if (staff == null)
                {
                    await roleMgr.CreateAsync(new IdentityRole(RoleNames.Staff));
                    Log.Debug("staff role is created");

                }
                if (user == null) 
                {
                    await roleMgr.CreateAsync(new IdentityRole(RoleNames.User));
                    Log.Debug("user role is created");

                }
                var alice = userMgr.FindByNameAsync("alice").Result;
                if (alice == null)
                {
                    alice = new ApplicationUser
                    {
                        UserName = "alice",
                        Email = "AliceSmith@email.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(alice, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                                new Claim(JwtClaimTypes.GivenName, "Alice"),
                                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                            }).Result;

                    await userMgr.AddToRoleAsync(alice,RoleNames.Staff);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("alice created");
                }
                else
                {
                    Log.Debug("alice already exists");
                }

                var bob = userMgr.FindByNameAsync("bob").Result;
                if (bob == null)
                {
                    bob = new ApplicationUser
                    {
                        UserName = "bob",
                        Email = "BobSmith@email.com",
                        EmailConfirmed = true
                    };
                    var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(bob, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Bob Smith"),
                                new Claim(JwtClaimTypes.GivenName, "Bob"),
                                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                                new Claim("location", "somewhere")
                            }).Result;

                    await userMgr.AddToRoleAsync(bob, RoleNames.Staff);

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("bob created");
                }
                else
                {
                    Log.Debug("bob already exists");
                }

                
                var dat = userMgr.FindByNameAsync("dat").Result;
                if (dat == null)
                {
                    dat = new ApplicationUser
                    {
                        UserName = "dat",
                        Email = "phamtuandat1a0@gmail.com",
                        EmailConfirmed = true
                    };
                    var result = userMgr.CreateAsync(dat, "Phamdat11a1$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(dat, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Pham Dat"),
                                new Claim(JwtClaimTypes.GivenName, "Dat"),
                                new Claim(JwtClaimTypes.FamilyName, "Pham"),
                                new Claim(JwtClaimTypes.WebSite, "http://DiyDevblog.com"),
                                new Claim("userId", dat.Id)
                            }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("Dat created");

                   
                    await userMgr.AddToRoleAsync(dat, RoleNames.Administrator);
                }
                else
                {
                    Log.Debug("Dat already exists");
                }

            }
        }
    }
}

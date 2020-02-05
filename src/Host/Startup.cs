// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Reflection;
using Host.AspNetCorePolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PolicyServer.Runtime.Client;

namespace Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(PolicyDatabaseContext).GetTypeInfo().Assembly.GetName().Name;
            // `AddControllersWithViews()` Calls AddAuthorization under the hood.
            // Below, in the call to `UseEndpoints(...)` we require authorization to all controllers (besides controllers/actions that have `[AllowAnonymous]`).
            services.AddControllersWithViews();

            // this sets up authentication - for this demo we simply use a local cookie
            // typically authentication would be done using an external provider
            services.AddAuthentication("Cookies")
                .AddCookie("Cookies");

            services.AddDbContext<PolicyDatabaseContext>(builder =>
                builder.UseMySql("Server=localhost;Port=3306;Database=RadiantGuild_Policy;Uid=RadiantGuild;Pwd=ipGRA82nkjUFjg6zQ*!NTgEcSex&rSoH;",
                    mysqlOptions => { mysqlOptions.MigrationsAssembly(migrationsAssembly); }));

            // this sets up the PolicyServer client library and policy provider
            services.AddDatabasePolicyServerClient()
                .AddAuthorizationPermissionPolicies();

            // this adds the necessary handler for our custom medication requirement
            services.AddTransient<IAuthorizationHandler, MedicationRequirementHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            // add this middleware to make roles and permissions available as claims
            // this is mainly useful for using the classic [Authorize(Roles="foo")] and IsInRole functionality
            // this is not needed if you use the client library directly or the new policy-based authorization framework in ASP.NET Core
            app.UsePolicyServerClaims();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });
        }
    }
}
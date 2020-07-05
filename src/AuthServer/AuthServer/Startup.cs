// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IdentityModel.Tokens.Jwt;
using AuthServer.BL.Interfaces;
using AuthServer.BL.Services;
using AuthServer.DAL.Extensions;
using AuthServer.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AuthServer.Extensions;
using AuthServer.Models;
using AuthServer.Validators;
using FluentValidation;
using IdentityServer4.Services;

namespace AuthServer
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add<ValidationFilter>();
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddFluentValidation();

            services.AddTransient<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddTransient<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            services.RegisterDbModule(_configuration.GetConnectionString("IdentityServerDBConnection"));
            
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton<IDatabaseInitializerService, DatabaseInitializerService>();
            
            services.AddAuthentication(_configuration);
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app)
        {
            InitializeDatabase(app);
            
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var service = serviceScope.ServiceProvider.GetRequiredService<IDatabaseInitializerService>();
            service.Initialize(serviceScope.ServiceProvider);
        }
    }
}
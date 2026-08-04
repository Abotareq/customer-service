using CustomerService.Application.Common.Interfaces.Authentication;
using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Infrastructure.Authintication;
using CustomerService.Infrastructure.Identity;
using CustomerService.Infrastructure.Persistence;
using CustomerService.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Identity.Core;
namespace CustomerService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<CustomerSupportDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork>(sp =>
                sp.GetRequiredService<CustomerSupportDbContext>());

            // Identity
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CustomerSupportDbContext>();

            // JWT settings binding
            services.Configure<JwtSettings>(
                configuration.GetSection(JwtSettings.SectionName));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // Authentication services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}

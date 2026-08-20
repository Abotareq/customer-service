using CustomerService.Application.Common.Interfaces.Authentication;
using CustomerService.Application.Common.Interfaces.Message;
using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Infrastructure.Authintication;
using CustomerService.Infrastructure.Email;
using CustomerService.Infrastructure.Identity;
using CustomerService.Infrastructure.Notifications;
using CustomerService.Infrastructure.Persistence;
using CustomerService.Infrastructure.Persistence.Interceptors;
using CustomerService.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Identity.Core;
using System;
using System.Collections.Generic;
using IEmailSender = CustomerService.Application.Common.Interfaces.IEmailSender;
using System.Text;
namespace CustomerService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            //event dispatcher
            services.AddScoped<DomainEventsDispatchInterceptor>();

            services.AddDbContext<CustomerSupportDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.AddInterceptors(sp.GetRequiredService<DomainEventsDispatchInterceptor>());
            });
            // Unit of Work
            services.AddScoped<IUnitOfWork>(sp =>
                sp.GetRequiredService<CustomerSupportDbContext>());

            // Identity
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

            })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CustomerSupportDbContext>()
                 .AddDefaultTokenProviders();

            // JWT settings binding
            services.Configure<JwtSettings>(
                configuration.GetSection(JwtSettings.SectionName));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            // Authentication services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            //email
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));
            services.AddScoped<IEmailSender, SmtpEmailSender>();
            //messeges
            services.AddSignalR();
            services.AddScoped<IMessageNotifier, MessageNotifier>();
            return services;
        }
    }
}
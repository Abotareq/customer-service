using CustomerService.Domain.Users;
using CustomerService.Domain.Users.Entites;
using CustomerService.Domain.Users.ValueObjects;
using CustomerService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure.Identity
{
    public static class TestUserSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<CustomerSupportDbContext>();

            await SeedUserAsync(userManager, dbContext, "agent@example.com", "AgentPass123!", "Agent",
                id => Agent.Create(id, "Test Agent", "agent@example.com")
                    .Match<ErrorOr.ErrorOr<User>>(user => user, errors => errors));

            await SeedUserAsync(userManager, dbContext, "manager@example.com", "ManagerPass123!", "Manager",
                id => Manager.Create(id, "Test Manager", "manager@example.com")
                    .Match<ErrorOr.ErrorOr<User>>(user => user, errors => errors));
        }

        private static async Task SeedUserAsync(
            UserManager<ApplicationUser> userManager,
            CustomerSupportDbContext dbContext,
            string email,
            string password,
            string role,
            Func<UserId, ErrorOr.ErrorOr<User>> createDomainUser)
        {
            var existing = await userManager.FindByEmailAsync(email);
            if (existing is not null) return;

            var userId = UserId.CreateUnique();

            var domainUserResult = createDomainUser(userId);
            if (domainUserResult.IsError) return;

            var identityUser = new ApplicationUser
            {
                Id = userId.Value,
                UserName = email,
                Email = email
            };

            var createResult = await userManager.CreateAsync(identityUser, password);
            if (!createResult.Succeeded) return;

            await userManager.AddToRoleAsync(identityUser, role);

            dbContext.Users.Add(domainUserResult.Value);
            await dbContext.SaveChangesAsync();
        }
    }
}

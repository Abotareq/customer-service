using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Domain.Request;
using CustomerService.Domain.Users;
using CustomerService.Domain.Users.Entites;
using CustomerService.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure.Persistence
{
    public sealed class CustomerSupportDbContext
     : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IUnitOfWork
    {
        public CustomerSupportDbContext(DbContextOptions<CustomerSupportDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Agent> Agents => Set<Agent>();
        public DbSet<Manager> Managers => Set<Manager>();
        public DbSet<Request> Requests => Set<Request>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // required — configures Identity's own tables

            builder.ApplyConfigurationsFromAssembly(typeof(CustomerSupportDbContext).Assembly);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

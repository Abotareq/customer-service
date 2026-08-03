using CustomerService.Domain.Users;
using CustomerService.Domain.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(UserId userId);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        void Delete(User user);
    }
}

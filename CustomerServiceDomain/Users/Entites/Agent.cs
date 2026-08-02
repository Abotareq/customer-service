using CustomerService.Domain.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Users.Entites
{
    public sealed class Agent : User
    {
        private Agent(UserId userId, string fullName, string email)
            : base(userId, fullName, email)
        {
        }

        private Agent() { }

        public static Agent Create(string fullName, string email)
        {
            return new Agent(UserId.CreateUnique(), fullName, email);
        }
    }
}

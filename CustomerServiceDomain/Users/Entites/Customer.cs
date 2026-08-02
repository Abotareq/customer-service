using CustomerService.Domain.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Users.Entites
{
    public sealed class Customer : User
    {
        private Customer(UserId userId, string fullName, string email)
            : base(userId, fullName, email)
        {
        }

        private Customer() { }

        public static Customer Create(string fullName, string email)
        {
            return new Customer(UserId.CreateUnique(), fullName, email);
        }
    }
}

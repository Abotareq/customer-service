using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
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

        public static ErrorOr<Customer> Create(string fullName, string email)
        {
            var errors = ValidateBasicInfo(fullName, email);
            if (errors.Count > 0)
                return errors;

            return new Customer(UserId.CreateUnique(), fullName, email);
        }
    }
}
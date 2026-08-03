using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Users.Entites
{
    public sealed class Manager : User
    {
        private Manager(UserId userId, string fullName, string email)
            : base(userId, fullName, email)
        {
        }

        private Manager() { }

        public static ErrorOr<Manager> Create(UserId userId, string fullName, string email)
        {
            var errors = ValidateBasicInfo(fullName, email);
            if (errors.Count > 0)
                return errors;

            return new Manager(userId, fullName, email);
        }
    }
}

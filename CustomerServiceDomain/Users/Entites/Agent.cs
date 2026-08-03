using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
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

        public static ErrorOr<Agent> Create(UserId userId, string fullName, string email)
        {
            var errors = ValidateBasicInfo(fullName, email);
            if (errors.Count > 0)
                return errors;

            return new Agent(userId, fullName, email);
        }
    }
}

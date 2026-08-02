using CustomerService.Domain.Users.ValueObjects;
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

        public static Manager Create(string fullName, string email)
        {
            return new Manager(UserId.CreateUnique(), fullName, email);
        }
    }
}

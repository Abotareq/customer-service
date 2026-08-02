using CustomerService.Domain.Common.Models;
using CustomerService.Domain.Users.Events;
using CustomerService.Domain.Users.ValueObjects;
using CustomerSupport.Domain.DomainErrors;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Users
{
    public abstract class User : AggregateRoot
    {
        public UserId UserId { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }

        protected User(UserId userId, string fullName, string email)
            : base(userId.Value)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
        }
        protected static List<Error> ValidateBasicInfo(string fullName, string email)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(fullName))
                errors.Add(Errors.User.FullNameIsRequired);

            if (string.IsNullOrWhiteSpace(email))
                errors.Add(Errors.User.EmailIsRequired);
            else if (!email.Contains('@'))
                errors.Add(Errors.User.InvalidEmailFormat);

            return errors;
        }
        protected User() { }

        public void Delete()
        {
            RaiseDomainEvent(new UserDeleted(UserId));
        }
    }
}

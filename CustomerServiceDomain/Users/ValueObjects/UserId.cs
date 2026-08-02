using CustomerService.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Users.ValueObjects
{
    public sealed class UserId : ValueObject
    {
        public Guid Value { get; }

        private UserId(Guid value)
        {
            Value = value;
        }

        public static UserId CreateUnique() => new(Guid.NewGuid());

        public static UserId Create(Guid value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

    }
}
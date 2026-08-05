using CustomerService.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Request.ValueObjects
{
    public sealed class RequestId : ValueObject
    {
        public Guid Value { get; }

        private RequestId(Guid value)
        {
            Value = value;
        }

        public static RequestId CreateUnique() => new(Guid.NewGuid());

        public static RequestId Create(Guid value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

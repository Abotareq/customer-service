using CustomerService.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Request.ValueObjects
{
    public sealed class LogId : ValueObject
    {
        public Guid Value { get; }

        private LogId(Guid value)
        {
            Value = value;
        }

        public static LogId CreateUnique() => new(Guid.NewGuid());

        public static LogId Create(Guid value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

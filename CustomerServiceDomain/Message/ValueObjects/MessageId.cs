using CustomerService.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Message.ValueObjects
{
    public sealed class MessageId : ValueObject
    {
        public Guid Value { get; }

        private MessageId(Guid value)
        {
            Value = value;
        }

        public static MessageId CreateUnique() => new(Guid.NewGuid());

        public static MessageId Create(Guid value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

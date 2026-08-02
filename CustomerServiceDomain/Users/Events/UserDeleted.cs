using CustomerService.Domain.Common.Interfaces;
using CustomerService.Domain.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Users.Events
{
    public sealed record UserDeleted(UserId UserId) : IDomainEvent;
}

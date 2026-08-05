using CustomerService.Domain.Common.Interfaces;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Request.Events
{
    public sealed record AdditionalInfoRequested(RequestId RequestId, UserId RequestedBy) : IDomainEvent;
}

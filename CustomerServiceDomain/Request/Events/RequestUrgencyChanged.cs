using CustomerService.Domain.Common.Interfaces;
using CustomerService.Domain.Request.Enums;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Request.Events
{
    public sealed record RequestUrgencyChanged(
      RequestId RequestId,
      Urgency PreviousUrgency,
      Urgency NewUrgency,
      UserId ChangedBy) : IDomainEvent;
}

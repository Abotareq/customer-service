using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Requests
{
    public sealed record LogResponse(
     Guid LogId,
     string FieldChanged,
     string OldValue,
     string NewValue,
     Guid? ChangedByUserId,
     DateTime Timestamp,
     string? Description);
}

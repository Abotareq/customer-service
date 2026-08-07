using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Requests
{
    public sealed record RequestResponse(
     Guid RequestId,
     string ReferenceNumber,
     Guid CustomerId,
     Guid? AgentId,
     string Status,
     string Urgency,
     string Category,
     string Description,
     DateTime CreatedAt,
     DateTime UpdatedAt);
}

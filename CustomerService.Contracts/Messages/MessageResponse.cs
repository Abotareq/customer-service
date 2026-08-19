using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Messages
{
    public sealed record MessageResponse(
     Guid MessageId,
     Guid RequestId,
     Guid SenderId,
     string Content,
     DateTime Timestamp);
}

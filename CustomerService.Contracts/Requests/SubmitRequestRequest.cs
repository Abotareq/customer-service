using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Requests
{
    public sealed record SubmitRequestRequest(
     string Urgency,
     string Category,
     string Description);
}

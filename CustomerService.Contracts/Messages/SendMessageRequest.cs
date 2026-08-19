using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Messages
{
    public sealed record SendMessageRequest(string Content);
}

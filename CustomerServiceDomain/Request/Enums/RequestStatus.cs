using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Request.Enums
{
    public enum RequestStatus
    {
        Submitted,
        Assigned,
        InProgress,
        WaitingOnCustomer,
        Completed,
        Reopened
    }
}

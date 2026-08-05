using CustomerService.Domain.Request.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Request
{
    public static class RequestStatusTransitionRules
    {
        private static readonly Dictionary<RequestStatus, RequestStatus[]> AllowedTransitions = new()
        {
            [RequestStatus.Submitted] = new[] { RequestStatus.Assigned },
            [RequestStatus.Assigned] = new[] { RequestStatus.InProgress, RequestStatus.WaitingOnCustomer },
            [RequestStatus.InProgress] = new[] { RequestStatus.WaitingOnCustomer, RequestStatus.Completed },
            [RequestStatus.WaitingOnCustomer] = new[] { RequestStatus.InProgress },
            [RequestStatus.Completed] = new[] { RequestStatus.Reopened },
            [RequestStatus.Reopened] = new[] { RequestStatus.Assigned }
        };

        public static bool CanTransition(RequestStatus from, RequestStatus to)
        {
            return AllowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
        }
    }
}

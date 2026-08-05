using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.DomainErrors
{

    public static partial class Errors
    {
        public static class Request
        {
            public static Error DescriptionIsRequired => Error.Validation(
                code: "Request.DescriptionIsRequired",
                description: "Description is required.");

            public static Error InvalidStatusTransition => Error.Validation(
                code: "Request.InvalidStatusTransition",
                description: "This status transition is not allowed.");

            public static Error AlreadyAssigned => Error.Conflict(
                code: "Request.AlreadyAssigned",
                description: "This request has already been assigned to an agent.");

            public static Error CannotMessageAfterCompletion => Error.Validation(
                code: "Request.CannotMessageAfterCompletion",
                description: "Cannot send messages on a completed request.");
            public static Error CannotAssignCompletedRequest => Error.Validation(
    code: "Request.CannotAssignCompletedRequest",
    description: "Cannot assign a completed request.");
        }
    }
}

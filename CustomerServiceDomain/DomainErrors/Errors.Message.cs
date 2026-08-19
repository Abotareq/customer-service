using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.DomainErrors
{

    public static partial class Errors
    {
        public static class Message
        {
            public static Error ContentIsRequired => Error.Validation(
                code: "Message.ContentIsRequired",
                description: "Message content is required.");

            public static Error CannotMessageAfterCompletion => Error.Validation(
                code: "Message.CannotMessageAfterCompletion",
                description: "Cannot send messages on a completed request.");
        }
    }
}

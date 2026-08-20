using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Users
{
    public sealed record ResetPasswordRequest(
       Guid UserId,
       string Token,
       string NewPassword);
}

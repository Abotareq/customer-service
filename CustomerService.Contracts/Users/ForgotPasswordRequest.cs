using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Users
{
    public sealed record ForgotPasswordRequest(string Email);
}

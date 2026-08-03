using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Authentication
{
    public sealed record RegisterRequest(
      string FullName,
      string Email,
      string Password);
}

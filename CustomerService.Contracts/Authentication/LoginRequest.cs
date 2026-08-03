using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Authentication
{
    public sealed record LoginRequest(
      string Email,
      string Password);
}

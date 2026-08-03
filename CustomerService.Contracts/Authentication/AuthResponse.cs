using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Authentication
{
    public sealed record AuthResponse(
      Guid UserId,
      string FullName,
      string Email,
      string Role,
      string AccessToken,
      string RefreshToken);
}

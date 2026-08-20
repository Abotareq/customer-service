using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Users
{
    public sealed record UserProfileResponse(
     Guid UserId,
     string FullName,
     string Email,
     string Role);
}

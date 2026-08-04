using CustomerService.Domain.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace CustomerService.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user, string role);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken);
    }
}

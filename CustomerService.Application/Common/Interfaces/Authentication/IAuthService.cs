using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Common.Interfaces.Authentication
{
    public interface IAuthService
    {
        Task<ErrorOr<(Guid UserId, string Role)>> RegisterIdentityUserAsync(
            string email, string password);

        Task<ErrorOr<(Guid UserId, string Role)>> ValidateCredentialsAsync(
            string email, string password);
    }
}

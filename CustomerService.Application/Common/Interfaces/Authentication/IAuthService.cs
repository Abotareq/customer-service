using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Common.Interfaces.Authentication
{
    public interface IAuthService
    {

        Task<ErrorOr<string>> RegisterIdentityUserAsync(Guid userId, string email, string password);
        Task<ErrorOr<(Guid UserId, string Role)>> ValidateCredentialsAsync(string email, string password);
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryTime);
        Task<ErrorOr<string>> ValidateStoredRefreshTokenAsync(Guid userId, string refreshToken);
        Task<ErrorOr<string>> GenerateEmailConfirmationTokenAsync(Guid userId);
        Task<ErrorOr<Success>> ConfirmEmailAsync(Guid userId, string token);
    }
}

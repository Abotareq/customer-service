using CustomerService.Application.Common.Interfaces.Authentication;
using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Contracts.Authentication;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandHandler
     : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RefreshTokenCommandHandler(
            IAuthService authService,
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _authService = authService;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<AuthResponse>> Handle(
            RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal is null)
                return Error.Unauthorized("Auth.InvalidAccessToken", "Access token is invalid.");

            var userIdClaim = principal.FindFirst("sub");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userIdGuid))
                return Error.Unauthorized("Auth.InvalidAccessToken", "Access token is invalid.");

            var roleResult = await _authService.ValidateStoredRefreshTokenAsync(userIdGuid, request.RefreshToken);
            if (roleResult.IsError)
                return roleResult.Errors;

            var role = roleResult.Value;

            var user = await _userRepository.GetByIdAsync(UserId.Create(userIdGuid));
            if (user is null)
                return Error.NotFound("Auth.UserNotFound", "User record not found.");

            var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user, role);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            await _authService.SaveRefreshTokenAsync(userIdGuid, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return new AuthResponse(
                user.UserId.Value, user.FullName, user.Email, role, newAccessToken, newRefreshToken);
        }
    }
}

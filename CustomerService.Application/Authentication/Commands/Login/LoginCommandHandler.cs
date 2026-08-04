using CustomerService.Application.Common.Interfaces.Authentication;
using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Contracts.Authentication;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.Login
{

    public sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, ErrorOr<AuthResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IAuthService authService,
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _authService = authService;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<AuthResponse>> Handle(
            LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate credentials against Identity
            var credentialsResult = await _authService.ValidateCredentialsAsync(
                request.Email, request.Password);

            if (credentialsResult.IsError)
                return credentialsResult.Errors;

            var (userId, role) = credentialsResult.Value;

            // 2. Fetch the domain User (need FullName, and the domain object itself for token claims)
            var user = await _userRepository.GetByIdAsync(UserId.Create(userId));

            if (user is null)
                return Error.NotFound("Auth.UserNotFound", "User record not found.");

            // 3. Issue tokens
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, role);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            return new AuthResponse(
                user.UserId.Value,
                user.FullName,
                user.Email,
                role,
                accessToken,
                refreshToken);
        }
    }
}

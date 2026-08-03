using CustomerService.Application.Common.Interfaces.Authentication;
using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Contracts.Authentication;
using CustomerService.Domain.Users.Entites;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.Register
{
    public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, ErrorOr<AuthResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterCommandHandler(
            IAuthService authService,
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _authService = authService;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<AuthResponse>> Handle(
            RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. Create the identity account (hashes password, assigns "Customer" role)
            var identityResult = await _authService.RegisterIdentityUserAsync(
                request.Email, request.Password);

            if (identityResult.IsError)
                return identityResult.Errors;

            var (userId, role) = identityResult.Value;

            // 2. Create the Domain aggregate (always Customer, per your decision)
            var customerResult = Customer.Create(request.FullName, request.Email);
            //error
            if (customerResult.IsError)
                //error
                return customerResult.Errors;
            //error
            var customer = customerResult.Value;

            await _userRepository.AddAsync(customer);

            // 3. Issue tokens
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(customer, role);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            return new AuthResponse(
                customer.UserId.Value,
                customer.FullName,
                customer.Email,
                role,
                accessToken,
                refreshToken);
        }
    }
}

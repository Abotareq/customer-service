using CustomerService.Application.Common.Interfaces.Authentication;
using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Contracts.Authentication;
using CustomerService.Domain.Users.Entites;
using CustomerService.Domain.Users.ValueObjects;
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
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            IAuthService authService,
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<AuthResponse>> Handle(
    RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. Generate the shared id up front
            var userId = UserId.CreateUnique();

            // 2. Create the Domain aggregate first (validates fullName/email before touching Identity)
            var customerResult = Customer.Create(userId, request.FullName, request.Email);
            if (customerResult.IsError)
                return customerResult.Errors;

            var customer = customerResult.Value;

            // 3. Create the identity account using the SAME id
            var identityResult = await _authService.RegisterIdentityUserAsync(
                userId.Value, request.Email, request.Password);

            if (identityResult.IsError)
                return identityResult.Errors;

            var role = identityResult.Value;

            // 4. Persist the domain aggregate
            await _userRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. Issue tokens
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

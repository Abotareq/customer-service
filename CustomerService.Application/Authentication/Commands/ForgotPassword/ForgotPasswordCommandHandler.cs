using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Common.Interfaces.Authentication;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.ForgotPassword
{
    public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ErrorOr<Success>>
    {
        private readonly IAuthService _authService;
        private readonly IApiUrlProvider _apiUrlProvider;

        public ForgotPasswordCommandHandler(IAuthService authService, IApiUrlProvider apiUrlProvider)
        {
            _authService = authService;
            _apiUrlProvider = apiUrlProvider;
        }

        public async Task<ErrorOr<Success>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.SendPasswordResetTokenAsync(request.Email, _apiUrlProvider.BaseUrl);
        }
    }
}

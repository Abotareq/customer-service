using CustomerService.Application.Common.Interfaces.Authentication;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.ConfirmEmail
{
    public sealed class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorOr<Success>>
    {
        private readonly IAuthService _authService;

        public ConfirmEmailCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ErrorOr<Success>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ConfirmEmailAsync(request.UserId, request.Token);
        }
    }
}

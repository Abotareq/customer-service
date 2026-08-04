using CustomerService.Contracts.Authentication;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
       string AccessToken,
       string RefreshToken) : IRequest<ErrorOr<AuthResponse>>;
}

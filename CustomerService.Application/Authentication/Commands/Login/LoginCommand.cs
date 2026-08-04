using CustomerService.Contracts.Authentication;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.Login
{
    public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<ErrorOr<AuthResponse>>;
}

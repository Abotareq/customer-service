using CustomerService.Contracts.Authentication;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.Register
{

    public sealed record RegisterCommand(
        string FullName,
        string Email,
        string Password) : IRequest<ErrorOr<AuthResponse>>;
}

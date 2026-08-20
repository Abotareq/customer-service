using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.ForgotPassword
{
    public sealed record ForgotPasswordCommand(string Email) : IRequest<ErrorOr<Success>>;
}

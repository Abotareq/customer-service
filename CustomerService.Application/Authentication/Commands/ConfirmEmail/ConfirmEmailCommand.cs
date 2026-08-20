using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(Guid UserId, string Token) : IRequest<ErrorOr<Success>>;
}

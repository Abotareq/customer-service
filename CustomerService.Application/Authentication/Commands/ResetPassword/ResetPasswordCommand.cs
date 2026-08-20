using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Authentication.Commands.ResetPassword
{
    public sealed record ResetPasswordCommand(
      Guid UserId,
      string Token,
      string NewPassword) : IRequest<ErrorOr<Success>>;
}

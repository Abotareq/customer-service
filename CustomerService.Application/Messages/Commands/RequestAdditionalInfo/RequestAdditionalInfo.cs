using CustomerService.Contracts.Messages;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Messages.Commands.RequestAdditionalInfo
{
    public sealed record RequestAdditionalInfoCommand(
    Guid RequestId,
    Guid RequestedBy,
    string Content) : IRequest<ErrorOr<MessageResponse>>;
}

using CustomerService.Contracts.Messages;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Messages.Commands.SendMessage
{
    public sealed record SendMessageCommand(
     Guid RequestId,
     Guid SenderId,
     string Content) : IRequest<ErrorOr<MessageResponse>>;
}

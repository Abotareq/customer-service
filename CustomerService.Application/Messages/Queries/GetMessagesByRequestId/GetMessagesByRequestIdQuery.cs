using CustomerService.Contracts.Messages;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Messages.Queries.GetMessagesByRequestId
{
    public sealed record GetMessagesByRequestIdQuery(
     Guid RequestId,
     Guid RequesterId,
     string RequesterRole) : IRequest<ErrorOr<List<MessageResponse>>>;
}

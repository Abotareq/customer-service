using CustomerService.Contracts.Requests;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.AssignRequest
{

    public sealed record AssignRequestCommand(
        Guid RequestId,
        Guid AgentId,
        Guid AssignedBy) : IRequest<ErrorOr<RequestResponse>>;
}

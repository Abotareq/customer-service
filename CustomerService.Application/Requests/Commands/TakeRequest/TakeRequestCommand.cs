using CustomerService.Contracts.Requests;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.TakeRequest
{
    public sealed record TakeRequestCommand(
     Guid RequestId,
     Guid AgentId) : IRequest<ErrorOr<RequestResponse>>;
}

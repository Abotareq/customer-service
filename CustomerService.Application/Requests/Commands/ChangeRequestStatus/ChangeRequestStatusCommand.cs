using CustomerService.Contracts.Requests;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.ChangeRequestStatus
{
    public sealed record ChangeRequestStatusCommand(
     Guid RequestId,
     string NewStatus,
     Guid ChangedBy) : IRequest<ErrorOr<RequestResponse>>;
}

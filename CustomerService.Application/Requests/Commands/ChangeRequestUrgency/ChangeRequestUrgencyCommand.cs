using CustomerService.Contracts.Requests;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.ChangeRequestUrgency
{
    public sealed record ChangeRequestUrgencyCommand(
    Guid RequestId,
    string NewUrgency,
    Guid ChangedBy) : IRequest<ErrorOr<RequestResponse>>;
}

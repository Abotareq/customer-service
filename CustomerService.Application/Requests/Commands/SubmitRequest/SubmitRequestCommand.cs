using CustomerService.Contracts.Requests;
using CustomerService.Domain.Request.Enums;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.SubmitRequest
{

    public sealed record SubmitRequestCommand(
       Guid CustomerId,
       string Urgency,
       string Category,
       string Description) : IRequest<ErrorOr<RequestResponse>>;
}

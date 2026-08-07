using CustomerService.Contracts.Requests;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Queries.GetRequestById
{
    public sealed record GetRequestByIdQuery(
    Guid RequestId,
    Guid RequesterId,
    string RequesterRole) : IRequest<ErrorOr<RequestResponse>>;
}

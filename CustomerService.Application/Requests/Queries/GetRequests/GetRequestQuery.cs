using CustomerService.Contracts.Requests;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Queries.GetRequestsQuery
{
    public sealed record GetRequestQuery(
      Guid? CustomerId,
      Guid? AgentId,
      bool? UnassignedOnly,
      string? Status,
      string? Urgency,
      string? Category,
      int PageNumber,
      int PageSize,
      Guid RequesterId,
      string RequesterRole) : IRequest<ErrorOr<PagedRequestsResponse>>;
}

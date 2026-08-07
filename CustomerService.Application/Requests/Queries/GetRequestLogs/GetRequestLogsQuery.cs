using CustomerService.Contracts.Requests;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Queries.GetRequestLogs
{
    public sealed record GetRequestLogsQuery(
      Guid RequestId,
      Guid RequesterId,
      string RequesterRole) : IRequest<ErrorOr<List<LogResponse>>>;
}

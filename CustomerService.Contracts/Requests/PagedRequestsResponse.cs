using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Contracts.Requests
{
    public sealed record PagedRequestsResponse(
       List<RequestResponse> Items,
       int TotalCount,
       int PageNumber,
       int PageSize);
}

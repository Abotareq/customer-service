using CustomerService.Contracts.Requests;
using CustomerService.Domain.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Common
{
    public static class RequestMapper
    {
        public static RequestResponse ToResponse(Request request)
        {
            return new RequestResponse(
                request.RequestId.Value,
                request.ReferenceNumber,
                request.CustomerId.Value,
                request.AgentId?.Value,
                request.Status.ToString(),
                request.Urgency.ToString(),
                request.Category.ToString(),
                request.Description,
                request.CreatedAt,
                request.UpdatedAt);
        }
    }
}

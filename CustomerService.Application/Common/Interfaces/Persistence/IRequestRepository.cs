using CustomerService.Domain.Request;
using CustomerService.Domain.Request.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Common.Interfaces.Persistence
{
    public interface IRequestRepository
    {
        Task<Request?> GetByIdAsync(RequestId requestId);
        Task AddAsync(Request request);

        Task<(List<Request> Items, int TotalCount)> GetFilteredAsync(
            Guid? customerId,
            Guid? agentId,
            bool? unassignedOnly,
            string? status,
            string? urgency,
            string? category,
            int pageNumber,
            int pageSize);
    }
}

using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Domain.Request;
using CustomerService.Domain.Request.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence.Repositories
{
    public sealed class RequestRepository : IRequestRepository
    {
        private readonly CustomerSupportDbContext _dbContext;

        public RequestRepository(CustomerSupportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Request?> GetByIdAsync(RequestId requestId)
        {
            return await _dbContext.Requests.FindAsync(requestId.Value);
        }

        public async Task AddAsync(Request request)
        {
            await _dbContext.Requests.AddAsync(request);
        }

        public async Task<(List<Request> Items, int TotalCount)> GetFilteredAsync(
            Guid? customerId,
            Guid? agentId,
            bool? unassignedOnly,
            string? status,
            string? urgency,
            string? category,
            int pageNumber,
            int pageSize)
        {
            var query = _dbContext.Requests.AsQueryable();

            if (customerId.HasValue)
                query = query.Where(r => r.CustomerId.Value == customerId.Value);

            if (agentId.HasValue)
                query = query.Where(r => r.AgentId != null && r.AgentId.Value == agentId.Value);

            if (unassignedOnly == true)
                query = query.Where(r => r.AgentId == null);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(r => r.Status.ToString() == status);

            if (!string.IsNullOrWhiteSpace(urgency))
                query = query.Where(r => r.Urgency.ToString() == urgency);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(r => r.Category.ToString() == category);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}

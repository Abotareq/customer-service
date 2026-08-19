using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Domain.Request;
using CustomerService.Domain.Request.Entites;
using CustomerService.Domain.Request.Enums;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
            return await _dbContext.Requests
                .Include(r => r.Logs)
                .FirstOrDefaultAsync(r => r.Id == requestId.Value);
        }
        public void AddLog(RequestId requestId, Log log)
        {
            _dbContext.Set<Log>().Add(log);
            _dbContext.Entry(log).Property("RequestId").CurrentValue = requestId.Value;
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
                query = query.Where(r => r.CustomerId == UserId.Create(customerId.Value));

            if (agentId.HasValue)
                query = query.Where(r => r.AgentId == UserId.Create(agentId.Value));

            if (unassignedOnly == true)
                query = query.Where(r => r.AgentId == null);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(r => r.Status == Enum.Parse<RequestStatus>(status));

            if (!string.IsNullOrWhiteSpace(urgency))
                query = query.Where(r => r.Urgency == Enum.Parse<Urgency>(urgency));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(r => r.Category == Enum.Parse<RequestCategory>(category));

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

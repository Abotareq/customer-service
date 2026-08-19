using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Domain.Message;
using CustomerService.Domain.Request.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence.Repositories
{
    public sealed class MessageRepository : IMessageRepository
    {
        private readonly CustomerSupportDbContext _dbContext;

        public MessageRepository(CustomerSupportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Message message)
        {
            await _dbContext.Messages.AddAsync(message);
        }

        public async Task<List<Message>> GetByRequestIdAsync(RequestId requestId)
        {
            return await _dbContext.Messages
                .Where(m => m.RequestId == requestId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }
}

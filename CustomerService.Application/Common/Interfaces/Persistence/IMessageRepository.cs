using CustomerService.Domain.Message;
using CustomerService.Domain.Request.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using CustomerService.Domain.Message;
namespace CustomerService.Application.Common.Interfaces.Persistence
{
    public interface IMessageRepository
    {
        Task AddAsync(CustomerService.Domain.Message.Message message);
        Task<List<CustomerService.Domain.Message.Message>> GetByRequestIdAsync(RequestId requestId);
    }
}

using CustomerService.Contracts.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Common.Interfaces.Message
{
    public interface IMessageNotifier
    {
        Task NotifyNewMessageAsync(Guid requestId, MessageResponse message, CancellationToken cancellationToken);
    }
}

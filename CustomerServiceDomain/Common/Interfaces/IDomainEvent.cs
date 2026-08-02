using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Common.Interfaces
{
    public interface IDomainEvent : INotification
    {
    }
}

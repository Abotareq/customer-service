using CustomerService.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Common.Models
{


    public abstract class AggregateRoot : Entity
    {
       private readonly List<IDomainEvent> _domainEvents = new();

        protected AggregateRoot(Guid id) : base(id) { }

        protected AggregateRoot() { }

        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}

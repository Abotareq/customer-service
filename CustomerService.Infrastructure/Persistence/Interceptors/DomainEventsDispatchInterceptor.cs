using CustomerService.Domain.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure.Persistence.Interceptors
{
    public sealed class DomainEventsDispatchInterceptor : SaveChangesInterceptor
    {
        private readonly IMediator _mediator;

        public DomainEventsDispatchInterceptor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext is null)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var aggregatesWithEvents = dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .Where(a => a.DomainEvents.Any())
                .ToList();

            var domainEvents = aggregatesWithEvents
                .SelectMany(a => a.DomainEvents)
                .ToList();

            foreach (var aggregate in aggregatesWithEvents)
            {
                aggregate.ClearDomainEvents();
            }

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}

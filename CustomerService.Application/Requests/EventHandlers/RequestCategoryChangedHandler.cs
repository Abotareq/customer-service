using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Domain.Request.Entites;
using CustomerService.Domain.Request.Enums;
using CustomerService.Domain.Request.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.EventHandlers
{
    public sealed class RequestCategoryChangedHandler : INotificationHandler<RequestCategoryChanged>
    {
        private readonly IRequestRepository _requestRepository;

        public RequestCategoryChangedHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public Task Handle(RequestCategoryChanged notification, CancellationToken cancellationToken)
        {
            var log = Log.Create(
                LogField.Category,
                oldValue: notification.PreviousCategory.ToString(),
                newValue: notification.NewCategory.ToString(),
                changedByUserId: notification.ChangedBy);

            _requestRepository.AddLog(notification.RequestId, log);

            return Task.CompletedTask;
        }
    }
}

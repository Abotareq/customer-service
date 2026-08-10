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

    public sealed class RequestUrgencyChangedHandler : INotificationHandler<RequestUrgencyChanged>
    {
        private readonly IRequestRepository _requestRepository;

        public RequestUrgencyChangedHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public Task Handle(RequestUrgencyChanged notification, CancellationToken cancellationToken)
        {
            var log = Log.Create(
                LogField.Urgency,
                oldValue: notification.PreviousUrgency.ToString(),
                newValue: notification.NewUrgency.ToString(),
                changedByUserId: notification.ChangedBy);

            _requestRepository.AddLog(notification.RequestId, log);

            return Task.CompletedTask;
        }
    }
}

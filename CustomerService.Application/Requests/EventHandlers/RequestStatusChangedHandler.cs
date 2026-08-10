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
    public sealed class RequestStatusChangedHandler : INotificationHandler<RequestStatusChanged>
    {
        private readonly IRequestRepository _requestRepository;

        public RequestStatusChangedHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public Task Handle(RequestStatusChanged notification, CancellationToken cancellationToken)
        {
            var log = Log.Create(
                LogField.Status,
                notification.PreviousStatus.ToString(),
                notification.NewStatus.ToString(),
                notification.ChangedBy);

            _requestRepository.AddLog(notification.RequestId, log);

            return Task.CompletedTask;
        }
    }
}

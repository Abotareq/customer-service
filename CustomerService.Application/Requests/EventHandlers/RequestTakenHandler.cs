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
    public sealed class RequestTakenHandler : INotificationHandler<RequestTaken>
    {
        private readonly IRequestRepository _requestRepository;

        public RequestTakenHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public Task Handle(RequestTaken notification, CancellationToken cancellationToken)
        {
            var assignmentLog = Log.Create(
                LogField.Assignment,
                oldValue: "Unassigned",
                newValue: notification.AgentId.Value.ToString(),
                changedByUserId: notification.AgentId);

            var statusLog = Log.Create(
                LogField.Status,
                oldValue: notification.PreviousStatus.ToString(),
                newValue: notification.NewStatus.ToString(),
                changedByUserId: notification.AgentId);

            _requestRepository.AddLog(notification.RequestId, assignmentLog);
            _requestRepository.AddLog(notification.RequestId, statusLog);

            return Task.CompletedTask;
        }
    }
}

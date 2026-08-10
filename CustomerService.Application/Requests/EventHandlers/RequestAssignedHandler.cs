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
    public sealed class RequestAssignedHandler : INotificationHandler<RequestAssigned>
    {
        private readonly IRequestRepository _requestRepository;

        public RequestAssignedHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public Task Handle(RequestAssigned notification, CancellationToken cancellationToken)
        {
            var log = Log.Create(
                LogField.Assignment,
                oldValue: notification.PreviousAgentId?.Value.ToString() ?? "Unassigned",
                newValue: notification.NewAgentId.Value.ToString(),
                changedByUserId: notification.AssignedBy);

            _requestRepository.AddLog(notification.RequestId, log);

            return Task.CompletedTask;
        }
    }
}

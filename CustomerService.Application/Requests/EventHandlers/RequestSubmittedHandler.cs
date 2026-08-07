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
    public sealed class RequestSubmittedHandler : INotificationHandler<RequestSubmitted>
    {
        private readonly IRequestRepository _requestRepository;

        public RequestSubmittedHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task Handle(RequestSubmitted notification, CancellationToken cancellationToken)
        {
            var request = await _requestRepository.GetByIdAsync(notification.RequestId);
            if (request is null) return;

            request.AppendLog(Log.Create(
                LogField.Status,
                oldValue: "N/A",
                newValue: RequestStatus.Submitted.ToString(),
                changedByUserId: notification.CustomerId,
                description: "Request submitted"));
        }
    }
}

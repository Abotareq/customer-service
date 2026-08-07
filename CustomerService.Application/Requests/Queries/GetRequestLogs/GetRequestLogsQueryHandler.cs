using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Contracts.Requests;
using CustomerService.Domain.DomainErrors;
using CustomerService.Domain.Request.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Queries.GetRequestLogs
{

    public sealed class GetRequestLogsQueryHandler
        : IRequestHandler<GetRequestLogsQuery, ErrorOr<List<LogResponse>>>
    {
        private readonly IRequestRepository _requestRepository;

        public GetRequestLogsQueryHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<ErrorOr<List<LogResponse>>> Handle(
            GetRequestLogsQuery request, CancellationToken cancellationToken)
        {
            var requestId = RequestId.Create(request.RequestId);
            var existingRequest = await _requestRepository.GetByIdAsync(requestId);

            if (existingRequest is null)
                return Error.NotFound("Request.NotFound", "Request not found.");

            var isAuthorized = request.RequesterRole switch
            {
                "Manager" => true,
                "Customer" => existingRequest.CustomerId.Value == request.RequesterId,
                "Agent" => existingRequest.AgentId?.Value == request.RequesterId,
                _ => false
            };

            if (!isAuthorized)
                return Errors.Request.NotAuthorizedToViewRequest;

            return existingRequest.Logs
                .Select(log => new LogResponse(
                    log.LogId.Value,
                    log.FieldChanged.ToString(),
                    log.OldValue,
                    log.NewValue,
                    log.ChangedByUserId?.Value,
                    log.Timestamp,
                    log.Description))
                .ToList();
        }
    }
}

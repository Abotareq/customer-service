using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Contracts.Messages;
using CustomerService.Domain.DomainErrors;
using CustomerService.Domain.Request.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Messages.Queries.GetMessagesByRequestId
{

    public sealed class GetMessagesByRequestIdQueryHandler
        : IRequestHandler<GetMessagesByRequestIdQuery, ErrorOr<List<MessageResponse>>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMessageRepository _messageRepository;

        public GetMessagesByRequestIdQueryHandler(
            IRequestRepository requestRepository, IMessageRepository messageRepository)
        {
            _requestRepository = requestRepository;
            _messageRepository = messageRepository;
        }

        public async Task<ErrorOr<List<MessageResponse>>> Handle(
            GetMessagesByRequestIdQuery request, CancellationToken cancellationToken)
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

            var messages = await _messageRepository.GetByRequestIdAsync(requestId);

            return messages
                .Select(m => new MessageResponse(
                    m.MessageId.Value, m.RequestId.Value, m.SenderId.Value, m.Content, m.Timestamp))
                .ToList();
        }
    }
}

using CustomerService.Application.Common.Interfaces.Message;
using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Contracts.Messages;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using CustomerService.Domain.Message;
namespace CustomerService.Application.Messages.Commands.RequestAdditionalInfo
{
    public sealed class RequestAdditionalInfoCommandHandler
     : IRequestHandler<RequestAdditionalInfoCommand, ErrorOr<MessageResponse>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageNotifier _messageNotifier;

        public RequestAdditionalInfoCommandHandler(
            IRequestRepository requestRepository,
            IMessageRepository messageRepository,
            IUnitOfWork unitOfWork,
            IMessageNotifier messageNotifier)
        {
            _requestRepository = requestRepository;
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _messageNotifier = messageNotifier;
        }

        public async Task<ErrorOr<MessageResponse>> Handle(
            RequestAdditionalInfoCommand request, CancellationToken cancellationToken)
        {
            var requestId = RequestId.Create(request.RequestId);
            var requestedBy = UserId.Create(request.RequestedBy);

            var existingRequest = await _requestRepository.GetByIdAsync(requestId);
            if (existingRequest is null)
                return Error.NotFound("Request.NotFound", "Request not found.");

            // 1. Transition the Request to WaitingOnCustomer (raises AdditionalInfoRequested event)
            var transitionResult = existingRequest.RequestAdditionalInfo(requestedBy);
            if (transitionResult.IsError)
                return transitionResult.Errors;

            // 2. Create the Message
            var messageResult = Message.Create(requestId, requestedBy, request.Content);
            if (messageResult.IsError)
                return messageResult.Errors;

            var message = messageResult.Value;

            await _messageRepository.AddAsync(message);

            // 3. Save both changes atomically — Request's status change + the new Message, one transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new MessageResponse(
                message.MessageId.Value, message.RequestId.Value, message.SenderId.Value,
                message.Content, message.Timestamp);

            // 4. Broadcast live, same as a normal SendMessage
            await _messageNotifier.NotifyNewMessageAsync(request.RequestId, response, cancellationToken);

            return response;
        }
    }

}
using CustomerService.Application.Common.Interfaces.Message;
using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Application.Messages.Commands.SendMessage;
using CustomerService.Contracts.Messages;
using CustomerService.Domain.DomainErrors;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Messagses.Commands.SendMessage
{
    public sealed class SendMessageCommandHandler
      : IRequestHandler<SendMessageCommand, ErrorOr<MessageResponse>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageNotifier _messageNotifier;

        public SendMessageCommandHandler(
            IMessageRepository messageRepository,
            IRequestRepository requestRepository,
            IUnitOfWork unitOfWork,
            IMessageNotifier messageNotifier)
        {
            _messageRepository = messageRepository;
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
            _messageNotifier = messageNotifier;
        }

        public async Task<ErrorOr<MessageResponse>> Handle(
            SendMessageCommand request, CancellationToken cancellationToken)
        {
            var requestId = RequestId.Create(request.RequestId);
            var senderId = UserId.Create(request.SenderId);

            var existingRequest = await _requestRepository.GetByIdAsync(requestId);
            if (existingRequest is null)
                return Error.NotFound("Request.NotFound", "Request not found.");

            if (!existingRequest.CanReceiveMessages())
                return Errors.Request.CannotMessageAfterCompletion;

            var messageResult = CustomerService.Domain.Message.Message.Create(requestId, senderId, request.Content);
            if (messageResult.IsError)
                return messageResult.Errors;

            var message = messageResult.Value;

            await _messageRepository.AddAsync(message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new MessageResponse(
                message.MessageId.Value, message.RequestId.Value, message.SenderId.Value,
                message.Content, message.Timestamp);

            await _messageNotifier.NotifyNewMessageAsync(request.RequestId, response, cancellationToken);

            return response;
        }
    }
}

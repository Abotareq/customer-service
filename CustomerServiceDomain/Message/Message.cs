using CustomerService.Domain.Common.Models;
using CustomerService.Domain.DomainErrors;
using CustomerService.Domain.Message.ValueObjects;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Message
{
    public sealed class Message : AggregateRoot
    {
        public MessageId MessageId { get; private set; }
        public RequestId RequestId { get; private set; }
        public UserId SenderId { get; private set; }
        public string Content { get; private set; }
        public DateTime Timestamp { get; private set; }

        private Message(MessageId messageId, RequestId requestId, UserId senderId, string content)
            : base(messageId.Value)
        {
            MessageId = messageId;
            RequestId = requestId;
            SenderId = senderId;
            Content = content;
            Timestamp = DateTime.UtcNow;
        }

        // EF Core
        private Message() { }

        public static ErrorOr<Message> Create(RequestId requestId, UserId senderId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return Errors.Message.ContentIsRequired;

            return new Message(MessageId.CreateUnique(), requestId, senderId, content);
        }
    }
}

using CustomerService.Domain.Common.Models;
using CustomerService.Domain.Request.Enums;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Request.Entites
{
    public sealed class Log : Entity
    {
        public LogId LogId { get; private set; }
        public LogField FieldChanged { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }
        public UserId? ChangedByUserId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string? Description { get; private set; }

        private Log(
            LogId logId,
            LogField fieldChanged,
            string oldValue,
            string newValue,
            UserId? changedByUserId,
            string? description)
            : base(logId.Value)
        {
            LogId = logId;
            FieldChanged = fieldChanged;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedByUserId = changedByUserId;
            Timestamp = DateTime.UtcNow;
            Description = description;
        }

        // EF Core
        private Log() { }

        public static Log Create(
            LogField fieldChanged,
            string oldValue,
            string newValue,
            UserId? changedByUserId,
            string? description = null)
        {
            return new Log(
                LogId.CreateUnique(), fieldChanged, oldValue, newValue, changedByUserId, description);
        }
    }
}

using CustomerService.Domain.Common.Models;
using CustomerService.Domain.DomainErrors;
using CustomerService.Domain.Request.Entites;
using CustomerService.Domain.Request.Enums;
using CustomerService.Domain.Request.Events;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Request
{
    public sealed class Request : AggregateRoot
    {
        private readonly List<Log> _logs = new();

        public RequestId RequestId { get; private set; }
        public string ReferenceNumber { get; private set; }
        public UserId CustomerId { get; private set; }
        public UserId? AgentId { get; private set; }
        public RequestStatus Status { get; private set; }
        public Urgency Urgency { get; private set; }
        public RequestCategory Category { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public IReadOnlyList<Log> Logs => _logs.AsReadOnly();

        private Request(
            RequestId requestId,
            string referenceNumber,
            UserId customerId,
            Urgency urgency,
            RequestCategory category,
            string description)
            : base(requestId.Value)
        {
            RequestId = requestId;
            ReferenceNumber = referenceNumber;
            CustomerId = customerId;
            Status = RequestStatus.Submitted;
            Urgency = urgency;
            Category = category;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // EF Core
        private Request() { }

        public static ErrorOr<Request> Submit(
            UserId customerId, Urgency urgency, RequestCategory category, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Errors.Request.DescriptionIsRequired;

            var requestId = RequestId.CreateUnique();
            var referenceNumber = GenerateReferenceNumber();

            var request = new Request(requestId, referenceNumber, customerId, urgency, category, description);

            request.RaiseDomainEvent(new RequestSubmitted(requestId, customerId));

            return request;
        }

        public ErrorOr<Success> Take(UserId agentId)
        {
            if (AgentId is not null)
                return Errors.Request.AlreadyAssigned;

            if (!RequestStatusTransitionRules.CanTransition(Status, RequestStatus.Assigned))
                return Errors.Request.InvalidStatusTransition;

            var previousStatus = Status;
            AgentId = agentId;
            Status = RequestStatus.Assigned;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new RequestTaken(RequestId, agentId, previousStatus, Status));

            return Result.Success;
        }

        public ErrorOr<Success> Assign(UserId agentId, UserId assignedBy)
        {
            if (Status == RequestStatus.Completed)
                return Errors.Request.CannotAssignCompletedRequest;

            var previousAgentId = AgentId;
            AgentId = agentId;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new RequestAssigned(RequestId, previousAgentId, agentId, assignedBy));

            return Result.Success;
        }

        public ErrorOr<Success> ChangeStatus(RequestStatus newStatus, UserId changedBy)
        {
            if (!RequestStatusTransitionRules.CanTransition(Status, newStatus))
                return Errors.Request.InvalidStatusTransition;

            var previousStatus = Status;
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new RequestStatusChanged(RequestId, previousStatus, newStatus, changedBy));

            return Result.Success;
        }

        public ErrorOr<Success> ChangeUrgency(Urgency newUrgency, UserId changedBy)
        {
            var previousUrgency = Urgency;
            Urgency = newUrgency;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new RequestUrgencyChanged(RequestId, previousUrgency, newUrgency, changedBy));

            return Result.Success;
        }

        public ErrorOr<Success> ChangeCategory(RequestCategory newCategory, UserId changedBy)
        {
            var previousCategory = Category;
            Category = newCategory;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new RequestCategoryChanged(RequestId, previousCategory, newCategory, changedBy));

            return Result.Success;
        }

        public ErrorOr<Success> RequestAdditionalInfo(UserId requestedBy)
        {
            if (!RequestStatusTransitionRules.CanTransition(Status, RequestStatus.WaitingOnCustomer))
                return Errors.Request.InvalidStatusTransition;

            Status = RequestStatus.WaitingOnCustomer;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new AdditionalInfoRequested(RequestId, requestedBy));

            return Result.Success;
        }

        public void AppendLog(Log log) => _logs.Add(log);

        public bool CanReceiveMessages() => Status != RequestStatus.Completed;

        // Cascade overrides — bypass normal transition rules, triggered by UserDeleted handlers
        public void ForceUnassignDueToAgentDeletion()
        {
            var previousStatus = Status;

            AgentId = null;
            Status = RequestStatus.Submitted;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new RequestStatusChanged(RequestId, previousStatus, Status, null));
        }

        public void ForceCompleteDueToCustomerDeletion()
        {
            var previousStatus = Status;
            Status = RequestStatus.Completed;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new RequestStatusChanged(RequestId, previousStatus, Status, null));
        }

        private static string GenerateReferenceNumber()
        {
            return $"REQ-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
    }
}

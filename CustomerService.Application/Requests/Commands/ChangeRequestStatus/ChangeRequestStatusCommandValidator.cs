using CustomerService.Domain.Request.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.ChangeRequestStatus
{
    public sealed class ChangeRequestStatusCommandValidator : AbstractValidator<ChangeRequestStatusCommand>
    {
        public ChangeRequestStatusCommandValidator()
        {
            RuleFor(x => x.RequestId).NotEmpty();
            RuleFor(x => x.ChangedBy).NotEmpty();

            RuleFor(x => x.NewStatus)
                .Must(s => Enum.TryParse<RequestStatus>(s, ignoreCase: true, out _))
                .WithMessage("NewStatus must be one of: Submitted, Assigned, InProgress, WaitingOnCustomer, Completed, Reopened.");
        }
    }
}

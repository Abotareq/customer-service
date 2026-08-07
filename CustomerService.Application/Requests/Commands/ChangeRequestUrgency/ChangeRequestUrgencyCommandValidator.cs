using CustomerService.Domain.Request.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.ChangeRequestUrgency
{
    public sealed class ChangeRequestUrgencyCommandValidator : AbstractValidator<ChangeRequestUrgencyCommand>
    {
        public ChangeRequestUrgencyCommandValidator()
        {
            RuleFor(x => x.RequestId).NotEmpty();
            RuleFor(x => x.ChangedBy).NotEmpty();

            RuleFor(x => x.NewUrgency)
                .Must(u => Enum.TryParse<Urgency>(u, ignoreCase: true, out _))
                .WithMessage("NewUrgency must be one of: Low, Medium, High, Critical.");
        }
    }
}

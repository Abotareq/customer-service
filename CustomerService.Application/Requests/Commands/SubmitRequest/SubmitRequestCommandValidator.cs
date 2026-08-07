using CustomerService.Domain.Request.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.SubmitRequest
{

    public sealed class SubmitRequestCommandValidator : AbstractValidator<SubmitRequestCommand>
    {
        public SubmitRequestCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);

            RuleFor(x => x.Urgency)
                .Must(u => Enum.TryParse<Urgency>(u, ignoreCase: true, out _))
                .WithMessage("Urgency must be one of: Low, Medium, High, Critical.");

            RuleFor(x => x.Category)
                .Must(c => Enum.TryParse<RequestCategory>(c, ignoreCase: true, out _))
                .WithMessage("Category must be one of: Technical, Billing, AccountAccess, FeatureRequest, General.");
        }
    }
}

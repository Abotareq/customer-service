using CustomerService.Domain.Request.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.ChangeRequestCategory
{
    public sealed class ChangeRequestCategoryCommandValidator : AbstractValidator<ChangeRequestCategoryCommand>
    {
        public ChangeRequestCategoryCommandValidator()
        {
            RuleFor(x => x.RequestId).NotEmpty();
            RuleFor(x => x.ChangedBy).NotEmpty();

            RuleFor(x => x.NewCategory)
                .Must(c => Enum.TryParse<RequestCategory>(c, ignoreCase: true, out _))
                .WithMessage("NewCategory must be one of: Technical, Billing, AccountAccess, FeatureRequest, General.");
        }
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Messages.Commands.RequestAdditionalInfo
{
    public sealed class RequestAdditionalInfoCommandValidator : AbstractValidator<RequestAdditionalInfoCommand>
    {
        public RequestAdditionalInfoCommandValidator()
        {
            RuleFor(x => x.RequestId).NotEmpty();
            RuleFor(x => x.RequestedBy).NotEmpty();
            RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
        }
    }

}

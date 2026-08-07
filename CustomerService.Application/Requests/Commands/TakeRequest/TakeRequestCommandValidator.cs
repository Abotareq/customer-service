using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.TakeRequest
{
    public sealed class TakeRequestCommandValidator : AbstractValidator<TakeRequestCommand>
    {
        public TakeRequestCommandValidator()
        {
            RuleFor(x => x.RequestId).NotEmpty();
            RuleFor(x => x.AgentId).NotEmpty();
        }
    }
}

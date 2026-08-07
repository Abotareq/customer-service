using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.AssignRequest
{
    public sealed class AssignRequestCommandValidator : AbstractValidator<AssignRequestCommand>
    {
        public AssignRequestCommandValidator()
        {
            RuleFor(x => x.RequestId).NotEmpty();
            RuleFor(x => x.AgentId).NotEmpty();
            RuleFor(x => x.AssignedBy).NotEmpty();
        }
    }
}

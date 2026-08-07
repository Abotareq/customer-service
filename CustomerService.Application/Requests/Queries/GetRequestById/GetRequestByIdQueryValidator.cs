using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Queries.GetRequestById
{
    public sealed class GetRequestByIdQueryValidator : AbstractValidator<GetRequestByIdQuery>
    {
        public GetRequestByIdQueryValidator()
        {
            RuleFor(x => x.RequestId).NotEmpty();
        }
    }
}

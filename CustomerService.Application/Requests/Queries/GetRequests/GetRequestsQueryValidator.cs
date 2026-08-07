using CustomerService.Application.Requests.Queries.GetRequestsQuery;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Queries.GetRequests
{
    public sealed class GetRequestsQueryValidator : AbstractValidator<GetRequestQuery>
    {
        public GetRequestsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}

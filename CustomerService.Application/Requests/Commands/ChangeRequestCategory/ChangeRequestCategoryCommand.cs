using CustomerService.Contracts.Requests;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.ChangeRequestCategory
{

    public sealed record ChangeRequestCategoryCommand(
        Guid RequestId,
        string NewCategory,
        Guid ChangedBy) : IRequest<ErrorOr<RequestResponse>>;
}

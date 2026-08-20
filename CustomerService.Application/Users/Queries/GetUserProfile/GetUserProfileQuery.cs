using CustomerService.Contracts.Users;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Users.Queries.GetUserProfile
{
    public sealed record GetUserProfileQuery(Guid UserId, string Role) : IRequest<ErrorOr<UserProfileResponse>>;
}

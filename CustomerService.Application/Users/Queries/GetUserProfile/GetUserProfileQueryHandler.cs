using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Contracts.Users;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Users.Queries.GetUserProfile
{
    public sealed class GetUserProfileQueryHandler
      : IRequestHandler<GetUserProfileQuery, ErrorOr<UserProfileResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserProfileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<UserProfileResponse>> Handle(
            GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(UserId.Create(request.UserId));
            if (user is null)
                return Error.NotFound("Auth.UserNotFound", "User not found.");

            return new UserProfileResponse(user.UserId.Value, user.FullName, user.Email, request.Role);
        }
    }
}

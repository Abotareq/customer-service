using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Application.Requests.Common;
using CustomerService.Contracts.Requests;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.AssignRequest
{
    public sealed class AssignRequestCommandHandler
      : IRequestHandler<AssignRequestCommand, ErrorOr<RequestResponse>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignRequestCommandHandler(
            IRequestRepository requestRepository, IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<RequestResponse>> Handle(
            AssignRequestCommand request, CancellationToken cancellationToken)
        {
            var requestId = RequestId.Create(request.RequestId);
            var agentId = UserId.Create(request.AgentId);
            var assignedBy = UserId.Create(request.AssignedBy);

            var existingRequest = await _requestRepository.GetByIdAsync(requestId);
            if (existingRequest is null)
                return Error.NotFound("Request.NotFound", "Request not found.");

            var assignResult = existingRequest.Assign(agentId, assignedBy);
            if (assignResult.IsError)
                return assignResult.Errors;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestMapper.ToResponse(existingRequest);
        }
    }
}

using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Application.Requests.Common;
using CustomerService.Contracts.Requests;
using CustomerService.Domain.Request.Enums;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.ChangeRequestUrgency
{
    public sealed class ChangeRequestUrgencyCommandHandler
      : IRequestHandler<ChangeRequestUrgencyCommand, ErrorOr<RequestResponse>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeRequestUrgencyCommandHandler(
            IRequestRepository requestRepository, IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<RequestResponse>> Handle(
            ChangeRequestUrgencyCommand request, CancellationToken cancellationToken)
        {
            var requestId = RequestId.Create(request.RequestId);
            var changedBy = UserId.Create(request.ChangedBy);
            var newUrgency = Enum.Parse<Urgency>(request.NewUrgency, ignoreCase: true);

            var existingRequest = await _requestRepository.GetByIdAsync(requestId);
            if (existingRequest is null)
                return Error.NotFound("Request.NotFound", "Request not found.");

            var changeResult = existingRequest.ChangeUrgency(newUrgency, changedBy);
            if (changeResult.IsError)
                return changeResult.Errors;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestMapper.ToResponse(existingRequest);
        }
    }
}

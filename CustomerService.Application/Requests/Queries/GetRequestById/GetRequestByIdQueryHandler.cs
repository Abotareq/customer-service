using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Application.Requests.Common;
using CustomerService.Contracts.Requests;
using CustomerService.Domain.DomainErrors;
using CustomerService.Domain.Request.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Queries.GetRequestById
{
    public sealed class GetRequestByIdQueryHandler
     : IRequestHandler<GetRequestByIdQuery, ErrorOr<RequestResponse>>
    {
        private readonly IRequestRepository _requestRepository;

        public GetRequestByIdQueryHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<ErrorOr<RequestResponse>> Handle(
     GetRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var requestId = RequestId.Create(request.RequestId);
            var existingRequest = await _requestRepository.GetByIdAsync(requestId);

            if (existingRequest is null)
                return Error.NotFound("Request.NotFound", "Request not found.");

            var isAuthorized = request.RequesterRole switch
            {
                "Manager" => true,
                "Customer" => existingRequest.CustomerId.Value == request.RequesterId,
                "Agent" => existingRequest.AgentId?.Value == request.RequesterId,
                _ => false
            };

            if (!isAuthorized)
                return Errors.Request.NotAuthorizedToViewRequest;

            return RequestMapper.ToResponse(existingRequest);
        }
    }
}

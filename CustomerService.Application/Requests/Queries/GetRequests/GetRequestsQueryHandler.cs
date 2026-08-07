using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Application.Requests.Common;
using CustomerService.Application.Requests.Queries.GetRequestsQuery;
using CustomerService.Contracts.Requests;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Queries.GetRequests
{
    public sealed class GetRequestsQueryHandler
     : IRequestHandler<GetRequestQuery, ErrorOr<PagedRequestsResponse>>
    {
        private readonly IRequestRepository _requestRepository;

        public GetRequestsQueryHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<ErrorOr<PagedRequestsResponse>> Handle(
            GetRequestQuery request, CancellationToken cancellationToken)
        {
            Guid? effectiveCustomerId = request.CustomerId;
            Guid? effectiveAgentId = request.AgentId;

            switch (request.RequesterRole)
            {
                case "Customer":
                    // A customer can only ever see their own requests, regardless of what was passed in
                    effectiveCustomerId = request.RequesterId;
                    effectiveAgentId = null;
                    break;

                case "Agent":
                    // An agent browsing without an explicit filter defaults to their own assigned requests
                    // (still allows unassignedOnly to work, since that's a distinct filter)
                    if (request.AgentId is null && request.UnassignedOnly != true)
                        effectiveAgentId = request.RequesterId;
                    break;

                case "Manager":
                    // No restriction — managers can view/filter across all requests
                    break;
            }

            var (items, totalCount) = await _requestRepository.GetFilteredAsync(
                effectiveCustomerId,
                effectiveAgentId,
                request.UnassignedOnly,
                request.Status,
                request.Urgency,
                request.Category,
                request.PageNumber,
                request.PageSize);

            var responseItems = items.Select(RequestMapper.ToResponse).ToList();

            return new PagedRequestsResponse(responseItems, totalCount, request.PageNumber, request.PageSize);
        }
    }

}

using CustomerService.Application.Common.Interfaces.Persistence;
using CustomerService.Application.Requests.Common;
using CustomerService.Contracts.Requests;
using CustomerService.Domain.Request;
using CustomerService.Domain.Request.Enums;
using CustomerService.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Requests.Commands.SubmitRequest
{
    public sealed class SubmitRequestCommandHandler
      : IRequestHandler<SubmitRequestCommand, ErrorOr<RequestResponse>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubmitRequestCommandHandler(
            IRequestRepository requestRepository, IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<RequestResponse>> Handle(
            SubmitRequestCommand request, CancellationToken cancellationToken)
        {
            var customerId = UserId.Create(request.CustomerId);
            var urgency = Enum.Parse<Urgency>(request.Urgency, ignoreCase: true);
            var category = Enum.Parse<RequestCategory>(request.Category, ignoreCase: true);

            var requestResult = Request.Submit(customerId, urgency, category, request.Description);

            if (requestResult.IsError)
                return requestResult.Errors;

            var newRequest = requestResult.Value;

            await _requestRepository.AddAsync(newRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestMapper.ToResponse(newRequest);
        }
       
    }
}

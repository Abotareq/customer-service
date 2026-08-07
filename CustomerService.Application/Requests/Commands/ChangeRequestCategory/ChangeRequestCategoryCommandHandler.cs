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

namespace CustomerService.Application.Requests.Commands.ChangeRequestCategory
{
    public sealed class ChangeRequestCategoryCommandHandler
     : IRequestHandler<ChangeRequestCategoryCommand, ErrorOr<RequestResponse>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeRequestCategoryCommandHandler(
            IRequestRepository requestRepository, IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<RequestResponse>> Handle(
            ChangeRequestCategoryCommand request, CancellationToken cancellationToken)
        {
            var requestId = RequestId.Create(request.RequestId);
            var changedBy = UserId.Create(request.ChangedBy);
            var newCategory = Enum.Parse<RequestCategory>(request.NewCategory, ignoreCase: true);

            var existingRequest = await _requestRepository.GetByIdAsync(requestId);
            if (existingRequest is null)
                return Error.NotFound("Request.NotFound", "Request not found.");

            var changeResult = existingRequest.ChangeCategory(newCategory, changedBy);
            if (changeResult.IsError)
                return changeResult.Errors;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestMapper.ToResponse(existingRequest);
        }
    }
}

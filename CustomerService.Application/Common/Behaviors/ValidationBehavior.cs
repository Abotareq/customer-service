using ErrorOr;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Common.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>
   : IPipelineBehavior<TRequest, TResponse>
   where TRequest : IRequest<TResponse>
   where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validator is null)
                return await next();

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
                return await next();

            var errors = validationResult.Errors
                .ConvertAll(e => Error.Validation(e.PropertyName, e.ErrorMessage));

            return (dynamic)errors;
        }
    }
}

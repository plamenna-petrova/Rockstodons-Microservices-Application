using Catalog.API.Application.Abstractions;
using Catalog.API.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Catalog.API.Application.Behaviors
{
    public class CustomValidationBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator> _validators;

        public CustomValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationFailures = _validators
                .Select(validator => validator.Validate(context))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure != null)
                .ToList();

            if (validationFailures.Any())
            {
                var error = string.Join("\r\n", validationFailures);
                throw new CatalogAPIException(error);
            }

            return next();
        }
    }
}

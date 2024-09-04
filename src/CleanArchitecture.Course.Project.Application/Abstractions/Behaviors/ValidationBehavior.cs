using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Application.Exceptions;
using FluentValidation;
using MediatR;
using ValidationException = CleanArchitecture.Course.Project.Application.Exceptions.ValidationException;

namespace CleanArchitecture.Course.Project.Application.Abstractions.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators
    ) : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
    {

        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            // var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var validationErrors = _validators
            .Select(_validator => _validator.Validate(context))
            .Where(result => result.Errors.Count != 0)
            .SelectMany(result => result.Errors)
            .Select(failure => new ValidateError(
                failure.PropertyName,
                failure.ErrorMessage
            ))
            .ToList();

            if(validationErrors.Count != 0)
            {
                throw new ValidationException(validationErrors);
            }

            return await next();
        }
    }
}
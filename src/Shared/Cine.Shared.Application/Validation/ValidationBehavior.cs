using FluentValidation;
using MediatR;

namespace Cine.Shared.Application.Validation
{
    public sealed class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> _validators)
        : IPipelineBehavior<TRequest, TResult> where TRequest : notnull
    {
        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(validator =>
                        validator.ValidateAsync(context, cancellationToken)));

                var errors = validationResults
                    .Where(result => result.Errors.Count > 0)
                    .SelectMany(result => result.Errors)
                    .ToList();

                if (errors.Count > 0)
                {
                    throw new ValidationException(errors);
                }
            }

            return await next();
        }
    }
}

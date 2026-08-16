using FluentValidation;
using MediatR;
using System.Text.Json;
using Task_Application.Common.Responses;

namespace Task_Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var validationFailures = validationResults
           .SelectMany(x => x.Errors)
           .Where(x => x is not null)
           .ToList();

        if (validationFailures.Any())
        {
            throw new ValidationException(validationFailures);
        }

        return await next();
    }
}
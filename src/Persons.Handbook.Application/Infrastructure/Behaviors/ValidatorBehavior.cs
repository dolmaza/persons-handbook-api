using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Persons.Handbook.Application.Infrastructure.Behaviors;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>[] _validators;
    public ValidatorBehavior(IValidator<TRequest>[] validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var failures = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            var errors = validationResult.Errors.Where(error => error != null);
            failures.AddRange(errors);
        }

        if (failures.Any())
        {
            throw new ValidationException($"Command Validation Errors for type {typeof(TRequest).Name}", failures);
        }

        var response = await next();
        return response;
    }
}
namespace Persons.Handbook.Application.Commands;

public record Command<TResponse>(Guid CorrelationId) : ICommand<TResponse>;
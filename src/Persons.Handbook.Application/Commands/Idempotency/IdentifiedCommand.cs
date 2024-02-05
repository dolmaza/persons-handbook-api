namespace Persons.Handbook.Application.Commands.Idempotency;

public record IdentifiedCommand<T, TR>(Guid CorrelationId, T Command) : ICommand<TR>
    where T : class, ICommand<TR>;
namespace Persons.Handbook.Infrastructure.Idempotency;

public record ClientRequest(Guid Id, string? Name, DateTime Time);
namespace Persons.Handbook.Infrastructure.Idempotency;

public interface IRequestManager
{
    Task<bool> ExistAsync(Guid id);

    Task CheckIfExistsAndCreateClientRequestAsync<T>(Guid id);
}
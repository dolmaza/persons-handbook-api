using Persons.Handbook.Domain.Exceptions;
using Persons.Handbook.Infrastructure.Context;

namespace Persons.Handbook.Infrastructure.Idempotency;

public class RequestManager : IRequestManager
{
    private readonly PersonsHandbookDbContext _context;

    public RequestManager(PersonsHandbookDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        var request = await _context.
            FindAsync<ClientRequest>(id);

        return request != null;
    }

    public async Task CheckIfExistsAndCreateClientRequestAsync<T>(Guid id)
    {
        var exists = await ExistAsync(id);

        var request = exists
            ? throw new PersonsHandbookDomainException($"Request with {id} already exists")
            : new ClientRequest(id, typeof(T).Name, DateTime.UtcNow);

        _context.Add(request);

        await _context.SaveChangesAsync();
    }
}
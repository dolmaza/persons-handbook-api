namespace Persons.Handbook.Domain.SeedWork;

public interface IRepository<TEntity, in TKey> where TEntity : Entity<TKey>, IAggregateRoot
{
    Task<TEntity?> FindByIdAsync(TKey id);

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    Task<bool> SaveChangesAsync();
}
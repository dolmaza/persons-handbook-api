using Microsoft.EntityFrameworkCore;
using Persons.Handbook.Domain.SeedWork;
using Persons.Handbook.Infrastructure.Context;

namespace Persons.Handbook.Infrastructure.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot
{
    private readonly PersonsHandbookDbContext _context;

    public Repository(PersonsHandbookDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> FindByIdAsync(TKey id)
    {
        return await _context.FindAsync<TEntity>(id); ;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
        foreach (var item in _context.ChangeTracker.Entries<Entity<TKey>>())
        {
            if (item.State == EntityState.Added)
            {
                item.Entity.CreateTime = DateTime.UtcNow;
                item.Entity.LastModifiedTime = DateTime.UtcNow;
            }
            else if (item.State == EntityState.Modified)
            {
                item.Entity.LastModifiedTime = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();

        return true;
    }
}
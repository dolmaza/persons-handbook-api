namespace Persons.Handbook.Domain.SeedWork;

public abstract class Entity<TKey>
{
    public TKey? Id { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime LastModifiedTime { get; set; }
}
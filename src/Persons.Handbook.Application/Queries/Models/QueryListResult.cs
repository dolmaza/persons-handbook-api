namespace Persons.Handbook.Application.Queries.Models;

public class QueryListResult<T>
{
    public QueryListResult(IList<T> items)
    {
        Items = items;
    }

    public IList<T>? Items { get; set; }
}
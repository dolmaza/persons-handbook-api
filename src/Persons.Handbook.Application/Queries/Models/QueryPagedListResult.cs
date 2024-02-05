namespace Persons.Handbook.Application.Queries.Models;

public class QueryPagedListResult<T> : QueryListResult<T>
{
    public QueryPagedListResult(int totalCount, IList<T> items) : base(items) => TotalCount = totalCount;

    public int TotalCount { get; set; }
}
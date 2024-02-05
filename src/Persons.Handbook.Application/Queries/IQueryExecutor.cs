namespace Persons.Handbook.Application.Queries;

public interface IQueryExecutor<in TRequest, TResponse> where TRequest : IQuery
{
    Task<TResponse?> Execute(TRequest request);
}
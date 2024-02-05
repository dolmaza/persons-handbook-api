using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persons.Handbook.Infrastructure.Context;

namespace Persons.Handbook.Application.Infrastructure.Behaviors;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
    private readonly PersonsHandbookDbContext _dbContext;

    public TransactionBehaviour(PersonsHandbookDbContext dbContext,
        ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentException(nameof(PersonsHandbookDbContext));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = default!;

        try
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"Begin transaction {typeof(TRequest).Name}");

                await _dbContext.BeginTransactionAsync(cancellationToken);

                response = await next();

                await _dbContext.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation($"Committed transaction {typeof(TRequest).Name}");

            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Rollback transaction executed {typeof(TRequest).Name}; \n {ex.ToString()}");

            _dbContext.RollbackTransaction();
            throw;
        }
    }
}
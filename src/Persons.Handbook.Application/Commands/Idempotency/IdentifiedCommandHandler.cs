﻿using MediatR;
using Persons.Handbook.Infrastructure.Idempotency;

namespace Persons.Handbook.Application.Commands.Idempotency;

/// <summary>
/// Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the cases where
/// a requestId sent by client is used to detect duplicate requests.
/// </summary>
/// <typeparam name="T">Type of the command handler that performs the operation if request is not duplicated</typeparam>
/// <typeparam name="TR">Return value of the inner command handler</typeparam>
public class IdentifiedCommandHandler<T, TR> : IRequestHandler<IdentifiedCommand<T, TR>, TR>
    where T : class, ICommand<TR>
{
    private readonly IMediator _mediator;
    private readonly IRequestManager _requestManager;

    public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager)
    {
        _mediator = mediator;
        _requestManager = requestManager;
    }

    /// <summary>
    /// Creates the result value to return if a previous request was found
    /// </summary>
    /// <returns></returns>
    protected virtual TR CreateResultForDuplicateRequest()
    {
        return default(TR)!;
    }

    /// <summary>
    /// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
    /// just enqueues the original inner command.
    /// </summary>
    /// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Return value of inner command or default value if request same ID was found</returns>
    public async Task<TR> Handle(IdentifiedCommand<T, TR> message, CancellationToken cancellationToken)
    {
        var alreadyExists = await _requestManager.ExistAsync(message.CorrelationId);
        if (alreadyExists)
        {
            return CreateResultForDuplicateRequest();
        }
        else
        {
            await _requestManager.CheckIfExistsAndCreateClientRequestAsync<T>(message.CorrelationId);

            // Send the embeded business command to mediator so it runs its related CommandHandler 
            var result = await _mediator.Send(message.Command as IRequest<TR>, cancellationToken);
            return result;
        }
    }
}
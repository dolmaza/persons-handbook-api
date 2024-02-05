using MediatR;

namespace Persons.Handbook.Application.Commands;

public interface ICommandHandler<in T, TR> : IRequestHandler<T, TR>
    where T : IRequest<TR>
{
    new Task<TR> Handle(T command, CancellationToken cancellationToken);
}
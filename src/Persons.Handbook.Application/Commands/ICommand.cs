using MediatR;

namespace Persons.Handbook.Application.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>
{

}
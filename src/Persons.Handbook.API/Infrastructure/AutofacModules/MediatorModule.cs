using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using Persons.Handbook.Application.Commands.Idempotency;
using Persons.Handbook.Application.Commands.PersonCommands.Create;
using Persons.Handbook.Application.Infrastructure.Behaviors;

namespace Persons.Handbook.API.Infrastructure.AutofacModules;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        // Register generic IdentifiedCommandHandler
        builder.RegisterGeneric(typeof(IdentifiedCommandHandler<,>))
            .As(typeof(IRequestHandler<,>));

        builder.RegisterAssemblyTypes(typeof(CreatePersonCommand).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));

        builder
            .RegisterAssemblyTypes(typeof(CreatePersonCommandValidator).GetTypeInfo().Assembly)
            .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
            .AsImplementedInterfaces();

        builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
    }
}
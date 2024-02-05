using System.Reflection;
using Autofac;
using Persons.Handbook.Application.Queries;
using Persons.Handbook.Application.Queries.PersonQueries.GetPersonById;

namespace Persons.Handbook.API.Infrastructure.AutofacModules;

public class ApplicationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(GetPersonByIdQueryExecutor).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IQueryExecutor<,>));
    }
}
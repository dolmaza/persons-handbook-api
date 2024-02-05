using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persons.Handbook.Domain.AggregatesModel.CityAggregate;
using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Domain.SeedWork;
using Persons.Handbook.Infrastructure.Context;
using Persons.Handbook.Infrastructure.Idempotency;
using Persons.Handbook.Infrastructure.Repositories;

namespace Persons.Handbook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomServices()
            .AddCustomDbContext(configuration);

        return services;
    }

    private static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PersonsHandbookDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(nameof(PersonsHandbookDbContext)));
            options.UseLazyLoadingProxies();
        });

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IConnectionRepository, ConnectionRepository>();

        services.AddScoped<IRequestManager, RequestManager>();

        return services;
    }
}
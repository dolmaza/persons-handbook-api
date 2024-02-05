using Persons.Handbook.Domain.AggregatesModel.CityAggregate;

namespace Persons.Handbook.Application.Commands.CityCommands.Create;

public record CreateCityCommand(Guid CorrelationId, string Name) : Command<int>(CorrelationId);

public class CreateCityCommandHandler : ICommandHandler<CreateCityCommand, int>
{
    private readonly ICityRepository _cityRepository;

    public CreateCityCommandHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<int> Handle(CreateCityCommand command, CancellationToken cancellationToken)
    {
        var city = new City(command.Name);

        await _cityRepository.AddAsync(city);
        await _cityRepository.SaveChangesAsync();

        return city.Id;
    }
}
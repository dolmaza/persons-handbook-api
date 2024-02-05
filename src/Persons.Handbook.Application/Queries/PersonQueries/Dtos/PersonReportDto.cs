using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;

namespace Persons.Handbook.Application.Queries.PersonQueries.Dtos;

public record PersonReportDto(int Id, string FirstName, string LastName, List<PersonReportItemDto> ReportItems);

public record PersonReportItemDto(string ConnectionType, int Count);
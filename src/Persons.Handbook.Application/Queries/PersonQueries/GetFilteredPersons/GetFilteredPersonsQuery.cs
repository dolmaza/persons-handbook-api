using Persons.Handbook.Application.Queries.Models;
using Persons.Handbook.Application.Queries.PersonQueries.Dtos;

namespace Persons.Handbook.Application.Queries.PersonQueries.GetFilteredPersons;

public record GetFilteredPersonsQuery(PersonFilterDto Filters, string? SearchValue, int Skip, int Take) : IQuery;
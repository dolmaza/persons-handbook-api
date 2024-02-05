using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;

namespace Persons.Handbook.Application.Queries.PersonQueries.Dtos;

public record PersonPhoneDto(string Number, PersonPhoneNumberType Type);
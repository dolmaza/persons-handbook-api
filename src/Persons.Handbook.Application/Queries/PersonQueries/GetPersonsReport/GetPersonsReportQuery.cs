namespace Persons.Handbook.Application.Queries.PersonQueries.GetPersonsReport;

public record GetPersonsReportQuery(int Skip, int Take) : IQuery;
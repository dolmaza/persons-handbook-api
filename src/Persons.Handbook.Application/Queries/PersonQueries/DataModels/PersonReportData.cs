namespace Persons.Handbook.Application.Queries.PersonQueries.DataModels;

public class PersonReportData
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int ColleagueCount { get; set; }
    public int FamiliarCount { get; set; }
    public int RelativeCount { get; set; }
    public int OtherCount { get; set; }
}
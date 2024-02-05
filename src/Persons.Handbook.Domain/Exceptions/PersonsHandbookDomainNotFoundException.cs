namespace Persons.Handbook.Domain.Exceptions;

public class PersonsHandbookDomainNotFoundException : Exception
{
    public PersonsHandbookDomainNotFoundException()
    {
        
    }

    public PersonsHandbookDomainNotFoundException(string message) : base(message)
    {
    }

    public PersonsHandbookDomainNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
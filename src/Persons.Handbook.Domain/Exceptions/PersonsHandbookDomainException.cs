namespace Persons.Handbook.Domain.Exceptions;

public class PersonsHandbookDomainException : Exception
{
    public PersonsHandbookDomainException()
    {
        
    }

    public PersonsHandbookDomainException(string message) : base(message)
    {
    }

    public PersonsHandbookDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
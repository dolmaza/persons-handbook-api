using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Domain.SeedWork;

namespace Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;

public class Connection : Entity<int>, IAggregateRoot
{
    public Connection()
    {

    }

    public Connection(int personId, int connectedPersonId, ConnectionType connectionType)
    {
        PersonId = personId;
        ConnectedPersonId = connectedPersonId;
        ConnectionType = connectionType;
    }

    public int PersonId { get; set; }
    public int ConnectedPersonId { get; set; }
    public ConnectionType ConnectionType { get; set; }

    public virtual Person? Person { get; set; }
    public virtual Person? ConnectedPerson { get; set; }
}
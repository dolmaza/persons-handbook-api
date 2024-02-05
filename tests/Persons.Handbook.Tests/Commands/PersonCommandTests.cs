using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using Persons.Handbook.Application.Commands.PersonCommands.Delete;
using Persons.Handbook.Application.Commands.PersonCommands.Update;
using Persons.Handbook.Application.Commands.PersonCommands.UploadPersonImage;
using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Domain.Exceptions;

namespace Persons.Handbook.Tests.Commands;

public class PersonCommandTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock;

    public PersonCommandTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
    }

    [Theory, AutoData]
    public async Task UpdatePerson_Handle_Success(UpdatePersonCommand command)
    {
        //Arrange
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
       
        var person = fixture.Create<Person>();

        _personRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(person);
        _personRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
        
        var handler = new UpdatePersonCommandHandler(_personRepositoryMock.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        Assert.True(result);
    }

    [Theory, AutoData]
    public async Task UpdatePerson_Handle_Fail(UpdatePersonCommand command)
    {
        //Arrange
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var person = fixture.Create<Person>();

        _personRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(person);
        _personRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(false);

        var handler = new UpdatePersonCommandHandler(_personRepositoryMock.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        Assert.False(result);
    }

    [Theory, AutoData]
    public async Task UpdatePerson_Handle_FailWhenPersonNotFound(UpdatePersonCommand command)
    {
        //Arrange
        _personRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        var handler = new UpdatePersonCommandHandler(_personRepositoryMock.Object);

        //Act & Assert
        await Assert.ThrowsAnyAsync<PersonsHandbookDomainNotFoundException>(async () => await handler.Handle(command, new CancellationToken()));
    }

    [Theory, AutoData]
    public async Task DeletePerson_Handle_Success(DeletePersonCommand command)
    {
        //Arrange
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var person = fixture.Create<Person>();

        _personRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(person);
        _personRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        var handler = new DeletePersonCommandHandler(_personRepositoryMock.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        Assert.True(result);
    }

    [Theory, AutoData]
    public async Task DeletePerson_Handle_Fail(DeletePersonCommand command)
    {
        //Arrange
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var person = fixture.Create<Person>();

        _personRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(person);
        _personRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(false);

        var handler = new DeletePersonCommandHandler(_personRepositoryMock.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        Assert.False(result);
    }

    [Theory, AutoData]
    public async Task DeletePerson_Handle_FailWhenPersonNotFound(DeletePersonCommand command)
    {
        //Arrange
        _personRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        var handler = new DeletePersonCommandHandler(_personRepositoryMock.Object);

        //Act & Assert
        await Assert.ThrowsAnyAsync<PersonsHandbookDomainNotFoundException>(async () => await handler.Handle(command, new CancellationToken()));
    }

    [Theory, AutoData]
    public async Task UploadPersonImage_Handle_Success(UploadPersonImageCommand command)
    {
        //Arrange
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var person = fixture.Create<Person>();

        _personRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(person);
        _personRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        var handler = new UploadPersonImageCommandHandler(_personRepositoryMock.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        Assert.True(result);
    }

    [Theory, AutoData]
    public async Task UploadPersonImage_Handle_Fail(UploadPersonImageCommand command)
    {
        //Arrange
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var person = fixture.Create<Person>();

        _personRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(person);
        _personRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(false);

        var handler = new UploadPersonImageCommandHandler(_personRepositoryMock.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        Assert.False(result);
    }

    [Theory, AutoData]
    public async Task UploadPersonImage_Handle_FailWhenPersonNotFound(UploadPersonImageCommand command)
    {
        //Arrange
        _personRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        var handler = new UploadPersonImageCommandHandler(_personRepositoryMock.Object);

        //Act & Assert
        await Assert.ThrowsAnyAsync<PersonsHandbookDomainNotFoundException>(async () => await handler.Handle(command, new CancellationToken()));
    }
}
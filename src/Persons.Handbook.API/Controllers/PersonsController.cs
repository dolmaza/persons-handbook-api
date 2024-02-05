using Microsoft.AspNetCore.Mvc;
using Persons.Handbook.Application.Commands.Idempotency;
using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Persons.Handbook.API.Infrastructure.FileManager;
using Persons.Handbook.Application.Commands.PersonCommands.Create;
using Persons.Handbook.Application.Commands.PersonCommands.Delete;
using Persons.Handbook.Application.Commands.PersonCommands.Update;
using Persons.Handbook.Application.Commands.PersonCommands.UploadPersonImage;
using Persons.Handbook.Application.Queries.Models;
using Persons.Handbook.Application.Queries.PersonQueries.Dtos;
using Persons.Handbook.Application.Queries.PersonQueries.GetFilteredPersons;
using Persons.Handbook.Application.Queries.PersonQueries.GetPersonById;
using Persons.Handbook.Application.Queries.PersonQueries.GetPersonsReport;

namespace Persons.Handbook.API.Controllers;

public class PersonsController : ApiBaseController
{
    private readonly IMediator _mediator;
    private readonly IFileManagerService _fileManagerService;
    private readonly ILogger<PersonsController> _logger;

    private readonly GetPersonByIdQueryExecutor _getPersonByIdQueryExecutor;
    private readonly GetFilteredPersonsQueryExecutor _getFilteredPersonsQueryExecutor;
    private readonly GetPersonsReportQueryExecutor _getPersonsReportQueryExecutor;

    public PersonsController
        (
            IMediator mediator, 
            IFileManagerService fileManagerService, 
            ILogger<PersonsController> logger, 
            GetPersonByIdQueryExecutor getPersonByIdQueryExecutor, 
            GetFilteredPersonsQueryExecutor getFilteredPersonsQueryExecutor, 
            GetPersonsReportQueryExecutor getPersonsReportQueryExecutor)
    {
        _mediator = mediator;
        _fileManagerService = fileManagerService;
        _logger = logger;
        _getPersonByIdQueryExecutor = getPersonByIdQueryExecutor;
        _getFilteredPersonsQueryExecutor = getFilteredPersonsQueryExecutor;
        _getPersonsReportQueryExecutor = getPersonsReportQueryExecutor;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PersonDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetFiltered
        (
            [FromQuery] PersonFilterDto filter,
            [FromQuery] string? searchValue,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 10
        )
    {
        var query = new GetFilteredPersonsQuery(filter, searchValue, skip, take);

        var result = await _getFilteredPersonsQueryExecutor.Execute(query);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PersonDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetPersonByIdQuery(id);

        var result = await _getPersonByIdQueryExecutor.Execute(query);

        return Ok(result);
    }

    [HttpGet("report")]
    [ProducesResponseType(typeof(PersonDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetReport
    (
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10
    )
    {
        var query = new GetPersonsReportQuery(skip, take);

        var result = await _getPersonsReportQueryExecutor.Execute(query);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePersonCommand command, [FromHeader] string requestId)
    {
        int commandResult = default;

        if (Guid.TryParse(requestId, out var correlationId) && correlationId != Guid.Empty)
        {
            var identifiedCommand = new IdentifiedCommand<CreatePersonCommand, int>(correlationId, command);
            commandResult = await _mediator.Send(identifiedCommand);
        }

        if (commandResult == default)
        {
            return BadRequest();
        }

        return Ok(new { id = commandResult });
    }
    
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdatePersonCommand command, [FromHeader] string requestId)
    {
        bool commandResult = default;

        if (Guid.TryParse(requestId, out var correlationId) && correlationId != Guid.Empty)
        {
            var identifiedCommand = new IdentifiedCommand<UpdatePersonCommand, bool>(correlationId, command);
            commandResult = await _mediator.Send(identifiedCommand);
        }

        if (commandResult == default)
        {
            return BadRequest();
        }

        return Ok();
    }
    
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update([FromBody] DeletePersonCommand command, [FromHeader] string requestId)
    {
        bool commandResult = default;

        if (Guid.TryParse(requestId, out var correlationId) && correlationId != Guid.Empty)
        {
            var identifiedCommand = new IdentifiedCommand<DeletePersonCommand, bool>(correlationId, command);
            commandResult = await _mediator.Send(identifiedCommand);
        }

        if (commandResult == default)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost("{id}/image-upload")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UploadPersonImage(int id, [FromForm] IFormFile file, [FromHeader] string requestId)
    {
        bool commandResult = default;

        var imageUrl = await _fileManagerService.UploadImageAsync(file);

        try
        {
            if (Guid.TryParse(requestId, out var correlationId) && correlationId != Guid.Empty)
            {
                var command = new UploadPersonImageCommand(correlationId, id, imageUrl);
                var identifiedCommand = new IdentifiedCommand<UploadPersonImageCommand, bool>(correlationId, command);
                commandResult = await _mediator.Send(identifiedCommand);
            }

            if (commandResult == default)
            {
                _fileManagerService.DeleteImage(imageUrl);

                return BadRequest();
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _fileManagerService.DeleteImage(imageUrl);

            _logger.LogError(ex, ex.Message);

            return BadRequest();
        }
    }
}

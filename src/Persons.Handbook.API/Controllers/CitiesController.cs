using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persons.Handbook.Application.Commands.Idempotency;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Persons.Handbook.Application.Commands.CityCommands.Create;

namespace Persons.Handbook.API.Controllers;

public class CitiesController : ApiBaseController
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreateCityCommand command, [FromHeader] string requestId)
    {
        int commandResult = default;

        if (Guid.TryParse(requestId, out var correlationId) && correlationId != Guid.Empty)
        {
            var identifiedCommand = new IdentifiedCommand<CreateCityCommand, int>(correlationId, command);
            commandResult = await _mediator.Send(identifiedCommand);
        }

        if (commandResult == default)
        {
            return BadRequest();
        }

        return Ok(new { id = commandResult });
    }
}


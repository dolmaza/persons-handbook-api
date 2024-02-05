using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persons.Handbook.Application.Commands.ConnectionCommands.ConnectPersons;
using Persons.Handbook.Application.Commands.Idempotency;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Persons.Handbook.Application.Commands.ConnectionCommands.RemoveConnection;

namespace Persons.Handbook.API.Controllers
{
    public class ConnectionsController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public ConnectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("connect-persons")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConnectPersons([FromBody] ConnectPersonsCommand command, [FromHeader] string requestId)
        {
            bool commandResult = default;

            if (Guid.TryParse(requestId, out var correlationId) && correlationId != Guid.Empty)
            {
                var identifiedCommand = new IdentifiedCommand<ConnectPersonsCommand, bool>(correlationId, command);
                commandResult = await _mediator.Send(identifiedCommand);
            }

            if (commandResult == default)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("remove-connection")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveConnection([FromBody] RemoveConnectionCommand command, [FromHeader] string requestId)
        {
            bool commandResult = default;

            if (Guid.TryParse(requestId, out var correlationId) && correlationId != Guid.Empty)
            {
                var identifiedCommand = new IdentifiedCommand<RemoveConnectionCommand, bool>(correlationId, command);
                commandResult = await _mediator.Send(identifiedCommand);
            }

            if (commandResult == default)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}

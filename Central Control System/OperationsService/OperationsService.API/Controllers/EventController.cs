using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Config;
using OperationsService.API.Model;
using OperationsService.Application.Commands.EventCommands.Delete;
using OperationsService.Application.Commands.EventCommands.Update;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Update;
using OperationsService.Application.Queries.EventQueries.Create;
using OperationsService.Application.Queries.EventQueries.GetAll;
using OperationsService.Application.Queries.EventQueries.GetByGID;
using OperationsService.Domain.Entities;

namespace OperationsService.API.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("operations/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "Event")]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllEventsQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetEventByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetEventByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO eventDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _mediator.Send(new CreateEventQuery() { Event = eventDto, Token = token }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDTO eventModel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            await _mediator.Send(new UpdateEventCommand() { Event = eventModel, Token = token });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteEvent(Guid gid)
        {
            await _mediator.Send(new DeleteEventCommand() { GID = gid });
            return NoContent();
        }
    }
}

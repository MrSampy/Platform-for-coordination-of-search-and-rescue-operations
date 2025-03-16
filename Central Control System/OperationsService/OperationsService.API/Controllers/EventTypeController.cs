using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Config;
using OperationsService.API.Model;
using OperationsService.Application.Commands.EventTypeCommands.Delete;
using OperationsService.Application.Commands.EventTypeCommands.Update;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Update;
using OperationsService.Application.Queries.EventTypeQueries.Create;
using OperationsService.Application.Queries.EventTypeQueries.GetAll;
using OperationsService.Application.Queries.EventTypeQueries.GetByGID;
using OperationsService.Domain.Entities;

namespace OperationsService.API.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("operations/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "EventType")]
    public class EventTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventTypeController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetEventTypes([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllEventTypesQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetEventTypeByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetEventTypeByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventType([FromBody] CreateEventTypeDTO eventTypeDto)
        {
            return Ok(await _mediator.Send(new CreateEventTypeQuery() { EventType = eventTypeDto }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEventType([FromBody] UpdateEventTypeDTO eventTypeModel)
        {
            await _mediator.Send(new UpdateEventTypeCommand() { EventType = eventTypeModel });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteEventType(Guid gid)
        {
            await _mediator.Send(new DeleteEventTypeCommand() { GID = gid });
            return NoContent();
        }
    }

}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Model;
using OperationsService.Application.Commands.EventStatusCommands.Delete;
using OperationsService.Application.Commands.EventStatusCommands.Update;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Update;
using OperationsService.Application.Queries.EventStatusQueries.Create;
using OperationsService.Application.Queries.EventStatusQueries.GetAll;
using OperationsService.Application.Queries.EventStatusQueries.GetByGID;
using OperationsService.Domain.Entities;

namespace OperationsService.API.Controllers
{
    [ApiController]
    [Route("operations/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "EventStatus")]
    public class EventStatusController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventStatusController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetEventStatuses([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllEventStatusesQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetEventStatusByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetEventStatusByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventStatus([FromBody] CreateEventStatusDTO eventStatusDto)
        {
            return Ok(await _mediator.Send(new CreateEventStatusQuery() { EventStatus = eventStatusDto }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEventStatus([FromBody] UpdateEventStatusDTO eventStatusModel)
        {
            await _mediator.Send(new UpdateEventStatusCommand() { EventStatus = eventStatusModel });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteEventStatus(Guid gid)
        {
            await _mediator.Send(new DeleteEventStatusCommand() { GID = gid });
            return NoContent();
        }
    }

}

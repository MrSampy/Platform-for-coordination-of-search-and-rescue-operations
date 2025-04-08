using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Config;
using OperationsService.API.Model;
using OperationsService.Application.Commands.ResourcesEventCommands.Delete;
using OperationsService.Application.Commands.ResourcesEventCommands.Update;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Update;
using OperationsService.Application.Queries.ResourcesEventQueries.Create;
using OperationsService.Application.Queries.ResourcesEventQueries.GetAll;
using OperationsService.Application.Queries.ResourcesEventQueries.GetByGID;
using OperationsService.Application.Queries.ResourcesEventQueries.GetEventsByResourceGID;
using OperationsService.Application.Queries.ResourcesEventQueries.GetResourcesByEventGID;
using OperationsService.Domain.Entities;

namespace OperationsService.API.Controllers
{
    [Authorize]
    [RequiresAuthHeader]
    [ApiController]
    [Route("operations/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "ResourcesEvent")]
    public class ResourcesEventController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ResourcesEventController(IMediator mediator) => _mediator = mediator;

        [HttpGet("by-event/{eventGID}")]
        public async Task<IActionResult> GetResourcesByEventGID(Guid eventGID, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetResourcesByEventGIDQuery { EventGID = eventGID }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("by-resource/{resourceGID}")]
        public async Task<IActionResult> GetEventsByResourceGID(Guid resourceGID, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetEventsByResourceGIDQuery { ResourceGID = resourceGID }, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetResourcesEvents([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllResourcesEventsQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetResourcesEventByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetResourcesEventByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateResourcesEvent([FromBody] CreateResourcesEventDTO resourcesEventDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _mediator.Send(new CreateResourcesEventQuery() { ResourcesEvent = resourcesEventDto, Token = token }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateResourcesEvent([FromBody] UpdateResourcesEventDTO resourcesEventModel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            await _mediator.Send(new UpdateResourcesEventCommand() { ResourcesEvent = resourcesEventModel, Token = token });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteResourcesEvent(Guid gid)
        {
            await _mediator.Send(new DeleteResourcesEventCommand() { GID = gid });
            return NoContent();
        }
    }

}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationsService.API.Config;
using OperationsService.API.Model;
using OperationsService.Application.Commands.ResourcesEventCommands.Delete;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.Queries.ResourcesEventQueries.Create;
using OperationsService.Application.Queries.ResourcesEventQueries.GetAll;
using OperationsService.Application.Queries.ResourcesEventQueries.GetByGID;
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

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteResourcesEvent(Guid gid)
        {
            await _mediator.Send(new DeleteResourcesEventCommand() { GID = gid });
            return NoContent();
        }
    }

}

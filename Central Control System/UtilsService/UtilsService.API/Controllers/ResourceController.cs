using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilsService.API.Model;
using UtilsService.Application.Commands.ResourceCommands.DeleteResource;
using UtilsService.Application.Commands.ResourceCommands.UpdateResource;
using UtilsService.Application.DTOs;
using UtilsService.Application.Queries.ResourcesQueries.CreateResource;
using UtilsService.Application.Queries.ResourcesQueries.GetAllResources;
using UtilsService.Application.Queries.ResourcesQueries.GetResourceByGid;
using UtilsService.Domain.Entities;

namespace UtilsService.API.Controllers
{
    [ApiController]
    [Route("utils/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [ApiExplorerSettings(GroupName = "Resource")]
    public class ResourceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ResourceController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetResources([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetAllResourcesQuery() { PaginationQuery = paginationQuery }, cancellationToken));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetResourceByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetResourceByGidQuery() { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateResource([FromBody] CreateResourceDTO resource)
        {
            return Ok(await _mediator.Send(new CreateResourceQuery() { Resource = resource }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateResource([FromBody] Resource resource)
        {
            await _mediator.Send(new UpdateResourceCommand() { Resource = resource });
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteResource(Guid gid)
        {
            await _mediator.Send(new DeleteResourceCommand() { GID = gid });
            return NoContent();
        }
    }
}

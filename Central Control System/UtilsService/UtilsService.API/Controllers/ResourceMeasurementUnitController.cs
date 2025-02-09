using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilsService.Application.Commands.ResourceUnitCommands.RemoveResourceFromUnit;
using UtilsService.Application.Queries.ResourceUnitQueries.AddResourceToUnit;
using UtilsService.Application.Queries.ResourceUnitQueries.GetResourcesByUnitGid;
using UtilsService.Application.Queries.ResourceUnitQueries.GetResourceUnitByGid;
using UtilsService.Application.Queries.ResourceUnitQueries.GetUnitsByResourceGid;
using UtilsService.Application.Queries.ResourceUnitQueries.IsResourceInUnit;
using UtilsService.Domain.Entities;

namespace UtilsService.API.Controllers
{
    [ApiController]
    [Route("utils/api/[controller]")]
    public class ResourceMeasurementUnitController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ResourceMeasurementUnitController(IMediator mediator) => _mediator = mediator;

        [HttpGet("by-unit/{unitGid}")]
        public async Task<IActionResult> GetResourcesByUnitGID(Guid unitGid, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetResourcesByUnitGidQuery { UnitGID = unitGid }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("by-resource/{resourceGid}")]
        public async Task<IActionResult> GetUnitsByResourceGID(Guid resourceGid, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetUnitsByResourceGidQuery { ResourceGID = resourceGid }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetResourceUnitByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new GetResourceUnitByGidQuery { GID = gid }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> AddResourceToUnit([FromBody] ResourceMeasurementUnit resourceUnit)
        {
            return Ok(await _mediator.Send(new AddResourceToUnitQuery { ResourceMeasurementUnit = resourceUnit }));
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> RemoveResourceFromUnit(Guid gid)
        {
            await _mediator.Send(new RemoveResourceFromUnitCommand { GID = gid });
            return NoContent();
        }

        [HttpPost("exists")]
        public async Task<IActionResult> IsResourceInUnit([FromBody] ResourceMeasurementUnit resourceUnit, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new IsResourceInUnitQuery { ResourceUnit = resourceUnit }, cancellationToken);
            return Ok(result);
        }
    }
}

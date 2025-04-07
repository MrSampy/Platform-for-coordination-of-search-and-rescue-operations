using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Utils;
using Gateway.DTO.DTOs.Utils.Create;
using Gateway.Integration.Api.Config;
using Gateway.Integration.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Integration.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("utils/api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [RequiresAuthHeader]
    [ApiExplorerSettings(GroupName = "ResourceMeasurementUnit")]
    public class ResourceMeasurementUnitController : ControllerBase
    {
        private readonly IUtilsGateway _utilsGateway;

        public ResourceMeasurementUnitController(IUtilsGateway utilsGateway)
        {
            _utilsGateway = utilsGateway;
        }

        [HttpGet("by-unit/{unitGid}")]
        public async Task<IActionResult> GetResourcesByUnitGID(Guid unitGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.GetResourcesByUnitGID(unitGid, cancellationToken, token));
        }

        [HttpGet("by-resource/{resourceGid}")]
        public async Task<IActionResult> GetUnitsByResourceGID(Guid resourceGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.GetUnitsByResourceGID(resourceGid, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetResourceUnitByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.GetResourceUnitByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> AddResourceToUnit([FromBody] CreateResourceUnitDTO resourceUnit)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.AddResourceToUnit(resourceUnit, token));
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> RemoveResourceFromUnit(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _utilsGateway.RemoveResourceFromUnit(gid, token);
            return NoContent();
        }

        [HttpPost("exists")]
        public async Task<IActionResult> IsResourceInUnit([FromBody] ResourceMeasurementUnitDTO resourceUnit, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.IsResourceInUnit(resourceUnit, token));
        }
    }
}

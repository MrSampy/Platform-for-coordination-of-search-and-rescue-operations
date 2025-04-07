using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Common;
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
    [ApiExplorerSettings(GroupName = "Resource")]
    public class ResourceController : ControllerBase
    {
        private readonly IUtilsGateway _utilsGateway;

        public ResourceController(IUtilsGateway utilsGateway)
        {
            _utilsGateway = utilsGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetResources([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.GetResources(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetResourceByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.GetResourceByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateResource([FromBody] CreateResourceDTO resource)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.CreateResource(resource, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateResource([FromBody] ResourceDTO resource)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _utilsGateway.UpdateResource(resource, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteResource(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _utilsGateway.DeleteResource(gid, token);
            return NoContent();
        }
    }
}

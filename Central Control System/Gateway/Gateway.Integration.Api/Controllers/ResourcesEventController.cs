using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Update;
using Gateway.Integration.Api.Config;
using Gateway.Integration.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Integration.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("gateway.integration.api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [RequiresAuthHeader]
    [ApiExplorerSettings(GroupName = "ResourcesEvent")]
    public class ResourcesEventController : ControllerBase
    {
        private readonly IOperationsGateway _operationsGateway;

        public ResourcesEventController(IOperationsGateway operationsGateway)
        {
            _operationsGateway = operationsGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetResourcesEvents([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetResourcesEvents(paginationQuery, cancellationToken, token));
        }

        [HttpGet("by-event/{eventGid}")]
        public async Task<IActionResult> GetResourcesByEventGID(Guid eventGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetResourcesByEventGID(eventGid, cancellationToken, token));
        }

        [HttpGet("by-resource/{resourceGid}")]
        public async Task<IActionResult> GetEventsByResourceGID(Guid resourceGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetEventsByResourceGID(resourceGid, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetResourcesEventByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetResourcesEventByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateResourcesEvent([FromBody] CreateResourcesEventDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.CreateResourcesEvent(dto, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateResourcesEvent([FromBody] UpdateResourcesEventDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.UpdateResourcesEvent(dto, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteResourcesEvent(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.DeleteResourcesEvent(gid, token);
            return NoContent();
        }
    }

}

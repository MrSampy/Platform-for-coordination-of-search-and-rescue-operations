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
    [ApiExplorerSettings(GroupName = "EventType")]
    public class EventTypeController : ControllerBase
    {
        private readonly IOperationsGateway _operationsGateway;

        public EventTypeController(IOperationsGateway operationsGateway)
        {
            _operationsGateway = operationsGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventTypes([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetEventTypes(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetEventTypeByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetEventTypeByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventType([FromBody] CreateEventTypeDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.CreateEventType(dto, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEventType([FromBody] UpdateEventTypeDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.UpdateEventType(dto, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteEventType(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.DeleteEventType(gid, token);
            return NoContent();
        }
    }

}

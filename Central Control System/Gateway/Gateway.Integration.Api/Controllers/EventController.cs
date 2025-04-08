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
    [ApiExplorerSettings(GroupName = "Event")]
    public class EventController : ControllerBase
    {
        private readonly IOperationsGateway _operationsGateway;

        public EventController(IOperationsGateway operationsGateway)
        {
            _operationsGateway = operationsGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetEvents(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetEventByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetEventByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.CreateEvent(dto, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.UpdateEvent(dto, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteEvent(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.DeleteEvent(gid, token);
            return NoContent();
        }
    }

}

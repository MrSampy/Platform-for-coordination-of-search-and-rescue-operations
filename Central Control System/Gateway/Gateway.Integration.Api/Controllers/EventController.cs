using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Request;
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
        private readonly IOperationsService _operationsService;

        public EventController(IOperationsGateway operationsGateway, IOperationsService operationsService)
        {
            _operationsGateway = operationsGateway;
            _operationsService = operationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetEvents(paginationQuery, cancellationToken, token));
        }
        [HttpPost("sort")]
        public async Task<IActionResult> GetEvents([FromBody] EventPaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsService.GetClearEvents(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetEventByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetEventByGID(gid, cancellationToken, token));
        }

        [HttpGet("by-eventstatus/{gid}")]
        public async Task<IActionResult> GetByStatusGID(Guid eventStatusGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetByStatusGID(eventStatusGid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.CreateEvent(dto, token));
        }

        [HttpPost("status-change")]
        public async Task<IActionResult> EventStatusChange([FromBody] EventStatusChangeRequest request)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsService.EventStatusChange(request, token);
            return NoContent();
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

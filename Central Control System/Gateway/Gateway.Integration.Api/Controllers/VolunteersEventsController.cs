using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Volunteers.Create;
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
    [ApiExplorerSettings(GroupName = "VolunteersEvents")]
    public class VolunteersEventsController : ControllerBase
    {
        private readonly IVolunteersGateway _volunteersGateway;

        public VolunteersEventsController(IVolunteersGateway volunteersGateway)
        {
            _volunteersGateway = volunteersGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetVolunteersEvents([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteersEvents(paginationQuery, cancellationToken, token));
        }

        [HttpGet("by-volunteer/{volunteerGid}")]
        public async Task<IActionResult> GetEventsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetEventsByVolunteerGID(volunteerGid, cancellationToken, token));
        }

        [HttpGet("by-Event/{EventGid}")]
        public async Task<IActionResult> GetVolunteersByEventGID(Guid EventGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteersByEventGID(EventGid, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetVolunteersEventsByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteersEventByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> AddVolunteerToEvent([FromBody] CreateVolunteersEventsDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.AddVolunteerToEvent(dto, token));
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> RemoveVolunteerFromEvent(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _volunteersGateway.RemoveVolunteerFromEvent(gid, token);
            return NoContent();
        }

        [HttpPost("exists")]
        public async Task<IActionResult> IsVolunteerinEvent([FromBody] CreateVolunteersEventsDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.IsVolunteerInEvent(dto, token));
        }
    }

}

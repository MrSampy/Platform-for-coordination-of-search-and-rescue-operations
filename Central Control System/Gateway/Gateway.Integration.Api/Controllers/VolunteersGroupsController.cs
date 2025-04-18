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
    [ApiExplorerSettings(GroupName = "VolunteersGroups")]
    public class VolunteersGroupsController : ControllerBase
    {
        private readonly IVolunteersGateway _volunteersGateway;

        public VolunteersGroupsController(IVolunteersGateway volunteersGateway)
        {
            _volunteersGateway = volunteersGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetVolunteersGroups([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteersGroups(paginationQuery, cancellationToken, token));
        }

        [HttpGet("by-volunteer/{volunteerGid}")]
        public async Task<IActionResult> GetGroupsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetGroupsByVolunteerGID(volunteerGid, cancellationToken, token));
        }

        [HttpGet("by-group/{groupGid}")]
        public async Task<IActionResult> GetVolunteersByGroupGID(Guid groupGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteersByGroupGID(groupGid, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetVolunteersGroupsByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteersGroupsByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> AddVolunteerToGroup([FromBody] CreateVolunteersGroupsDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.AddVolunteerToGroup(dto, token));
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> RemoveVolunteerFromGroup(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _volunteersGateway.RemoveVolunteerFromGroup(gid, token);
            return NoContent();
        }

        [HttpPost("exists")]
        public async Task<IActionResult> IsVolunteerinGroup([FromBody] CreateVolunteersGroupsDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.IsVolunteerinGroup(dto, token));
        }
    }

}

using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Volunteers.Create;
using Gateway.DTO.DTOs.Volunteers.Update;
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
    [ApiExplorerSettings(GroupName = "Volunteer")]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteersGateway _volunteersGateway;

        public VolunteerController(IVolunteersGateway volunteersGateway)
        {
            _volunteersGateway = volunteersGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetVolunteers([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteers(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetVolunteerByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteerByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateVolunteer([FromBody] CreateVolunteerDTO volunteer)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.CreateVolunteer(volunteer, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVolunteer([FromBody] UpdateVolunteerDTO volunteer)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _volunteersGateway.UpdateVolunteer(volunteer, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteVolunteer(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _volunteersGateway.DeleteVolunteer(gid, token);
            return NoContent();
        }
    }

}

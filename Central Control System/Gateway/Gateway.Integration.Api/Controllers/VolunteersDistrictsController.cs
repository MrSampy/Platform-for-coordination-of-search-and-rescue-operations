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
    [ApiExplorerSettings(GroupName = "VolunteersDistricts")]
    public class VolunteersDistrictsController : ControllerBase
    {
        private readonly IVolunteersGateway _volunteersGateway;

        public VolunteersDistrictsController(IVolunteersGateway volunteersGateway)
        {
            _volunteersGateway = volunteersGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetVolunteersDistricts([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteersDistricts(paginationQuery, cancellationToken, token));
        }

        [HttpGet("by-volunteer/{volunteerGid}")]
        public async Task<IActionResult> GetDistrictsByVolunteerGID(Guid volunteerGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetDistrictsByVolunteerGID(volunteerGid, cancellationToken, token));
        }

        [HttpGet("by-district/{districtGid}")]
        public async Task<IActionResult> GetVolunteersByDistrictGID(Guid districtGid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteersByDistrictGID(districtGid, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetVolunteersDistrictByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.GetVolunteersDistrictByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateVolunteersDistrict([FromBody] CreateVolunteersDistrictsDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.CreateVolunteersDistrict(dto, token));
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteVolunteersDistrict(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _volunteersGateway.DeleteVolunteersDistrict(gid, token);
            return NoContent();
        }

        [HttpPost("exists")]
        public async Task<IActionResult> IsVolunteerinDistrict([FromBody] CreateVolunteersDistrictsDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _volunteersGateway.IsVolunteerinDistrict(dto, token));
        }
    }

}

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
    [Route("gateway.integration.api/[controller]")]
    [RateLimit(MaxRequests = 10, TimeWindowInSeconds = 1)]
    [RequiresAuthHeader]
    [ApiExplorerSettings(GroupName = "District")]
    public class DistrictController : ControllerBase
    {
        private readonly IUtilsGateway _utilsGateway;

        public DistrictController(IUtilsGateway utilsGateway)
        {
            _utilsGateway = utilsGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetDistricts([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.GetDistricts(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetDistrictByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.GetDistrictByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDistrict([FromBody] CreateDistrictDTO district)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.CreateDistrict(district, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDistrict([FromBody] DistrictDTO district)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _utilsGateway.UpdateDistrict(district, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteDistrict(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _utilsGateway.DeleteDistrict(gid, token);
            return NoContent();
        }
    }
}

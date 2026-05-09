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
    [ApiExplorerSettings(GroupName = "MeasurementUnit")]
    public class MeasurementUnitController : ControllerBase
    {
        private readonly IUtilsGateway _utilsGateway;

        public MeasurementUnitController(IUtilsGateway utilsGateway)
        {
            _utilsGateway = utilsGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetMeasurementUnits([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.GetMeasurementUnits(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetMeasurementUnitByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.GetMeasurementUnitByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeasurementUnit([FromBody] CreateMeasurementUnitDTO measurementUnit)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _utilsGateway.CreateMeasurementUnit(measurementUnit, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMeasurementUnit([FromBody] MeasurementUnitDTO measurementUnit)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _utilsGateway.UpdateMeasurementUnit(measurementUnit, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteMeasurementUnit(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _utilsGateway.DeleteMeasurementUnit(gid, token);
            return NoContent();
        }
    }
}

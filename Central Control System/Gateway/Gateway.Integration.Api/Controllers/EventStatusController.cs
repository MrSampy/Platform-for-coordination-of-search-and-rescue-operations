using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations.Create;
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
    [ApiExplorerSettings(GroupName = "EventStatus")]
    public class EventStatusController : ControllerBase
    {
        private readonly IOperationsGateway _operationsGateway;

        public EventStatusController(IOperationsGateway operationsGateway)
        {
            _operationsGateway = operationsGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventStatuses([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetEventStatuss(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetEventStatusByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetEventStatusByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventStatus([FromBody] CreateEventStatusDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.CreateEventStatus(dto, token));
        }

    }
}

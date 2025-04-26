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
    [ApiExplorerSettings(GroupName = "Group")]
    public class GroupController : ControllerBase
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IOperationsService _operationsService;

        public GroupController(IOperationsGateway operationsGateway, IOperationsService operationsService)
        {
            _operationsGateway = operationsGateway;
            _operationsService = operationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetGroups(paginationQuery, cancellationToken, token));
        }

        [HttpGet("byEventGID/{eventGID}")]
        public async Task<IActionResult> GetGroupsByEventGID(Guid eventGID, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsService.GetGroupsByEventGID(eventGID, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetGroupByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetGroupByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.CreateGroup(dto, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroupDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.UpdateGroup(dto, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteGroup(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsService.DeleteGroup(gid, token);
            return NoContent();
        }
    }

}

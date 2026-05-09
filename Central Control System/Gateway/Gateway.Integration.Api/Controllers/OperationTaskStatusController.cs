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
    [ApiExplorerSettings(GroupName = "OperationTaskStatus")]
    public class OperationTaskStatusController : ControllerBase
    {
        private readonly IOperationsGateway _operationsGateway;

        public OperationTaskStatusController(IOperationsGateway operationsGateway)
        {
            _operationsGateway = operationsGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetOperationTaskStatuses([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetOperationTaskStatuss(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetOperationTaskStatusByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetOperationTaskStatusByGID(gid, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOperationTaskStatus([FromBody] CreateOperationTaskStatusDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.CreateOperationTaskStatus(dto, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOperationTaskStatus([FromBody] UpdateOperationTaskStatusDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.UpdateOperationTaskStatus(dto, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteOperationTaskStatus(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.DeleteOperationTaskStatus(gid, token);
            return NoContent();
        }
    }

}

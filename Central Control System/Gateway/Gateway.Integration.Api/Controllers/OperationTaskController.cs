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
    [ApiExplorerSettings(GroupName = "OperationTask")]
    public class OperationTaskController : ControllerBase
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IOperationsService _operationsService;

        public OperationTaskController(IOperationsGateway operationsGateway, IOperationsService operationsService)
        {
            _operationsGateway = operationsGateway;
            _operationsService = operationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOperationTasks([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetOperationTasks(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetOperationTaskByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetOperationTaskByGID(gid, cancellationToken, token));
        }

        [HttpGet("byGroupGID/{groupGID}")]
        public async Task<IActionResult> GetOperationTasksByGroupGID(Guid groupGID, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsService.GetOperationTasksByGroupGID(groupGID, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOperationTask([FromBody] CreateOperationTaskDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.CreateOperationTask(dto, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOperationTask([FromBody] UpdateOperationTaskDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.UpdateOperationTask(dto, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteOperationTask(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.DeleteOperationTask(gid, token);
            return NoContent();
        }
    }

}

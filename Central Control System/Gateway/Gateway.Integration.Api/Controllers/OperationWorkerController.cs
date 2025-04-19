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
    [ApiExplorerSettings(GroupName = "OperationWorker")]
    public class OperationWorkerController : ControllerBase
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IOperationsService _operationsService;

        public OperationWorkerController(IOperationsGateway operationsGateway, IOperationsService operationsService)
        {
            _operationsGateway = operationsGateway;
            _operationsService = operationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOperationWorkers([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetOperationWorkers(paginationQuery, cancellationToken, token));
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetOperationWorkerByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.GetOperationWorkerByGID(gid, cancellationToken, token));
        }

        [HttpGet("byRole/{roleName}")]
        public async Task<IActionResult> GetOperationWorkerByGID(string roleName, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsService.GetWorkersByRole(new DTO.DTOs.Operations.Request.GetOperationWorkersByRoleName { RoleName = roleName }, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOperationWorker([FromBody] CreateOperationWorkerDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _operationsGateway.CreateOperationWorker(dto, token));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOperationWorker([FromBody] UpdateOperationWorkerDTO dto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.UpdateOperationWorker(dto, token);
            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteOperationWorker(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _operationsGateway.DeleteOperationWorker(gid, token);
            return NoContent();
        }
    }

}

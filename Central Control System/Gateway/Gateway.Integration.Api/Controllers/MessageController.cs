using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Request;
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
    [ApiExplorerSettings(GroupName = "Message")]
    public class MessageController : ControllerBase
    {
        private readonly IOperationsGateway _operationsGateway;
        private readonly IOperationsService _operationsService;
        public MessageController(IOperationsGateway operationsGateway, IOperationsService operationsService)
        {
            _operationsGateway = operationsGateway;
            _operationsService = operationsService;
        }

        [HttpGet("{gid}")]
        public async Task<IActionResult> GetMessageByGID(Guid gid, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _operationsGateway.GetMessageByGID(gid, cancellationToken, token));
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetMessages([FromBody] MessagePaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _operationsService.GetMessages(paginationQuery, cancellationToken, token));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDTO messageDTO)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _operationsGateway.CreateMessage(messageDTO, token));
        }

        [HttpPut("read/{gid}")]
        public async Task<IActionResult> ReadMessage(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            await _operationsGateway.ReadMessage(gid, token);

            return NoContent();
        }

        [HttpDelete("{gid}")]
        public async Task<IActionResult> DeleteMessage(Guid gid)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            await _operationsGateway.DeleteMessage(gid, token);
            return NoContent();
        }
    }
}
